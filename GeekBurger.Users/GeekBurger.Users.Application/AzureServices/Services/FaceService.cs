﻿using GeekBurger.Users.Core.Configs;
using GeekBurger.Users.Core.Domains;
using GeekBurger.Users.Data;
using Microsoft.ProjectOxford.Face;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Users.Application.AzureServices.Services
{
    public class FaceService : IFaceService
    {
        private readonly string _faceListId;
        private readonly FaceServiceClient _faceServiceClient;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private List<User> _detectedUsers;
        private List<Task> _taskExceptions;
        private IServiceBus _serviceBus;
        private Task _lastTask;


        public FaceService(
            IUserService userService, 
            IUserRepository userRepository,
            IServiceBus serviceBus)
        {
            _faceListId = ConfigurationManager.Configuration["azureConfig:faceListId"];
            _faceServiceClient = new FaceServiceClient(ConfigurationManager.Configuration["azureConfig:key1"], ConfigurationManager.Configuration["azureConfig:endpoint"]);
            _detectedUsers = new List<User>();
            _userService = userService;
            _userRepository = userRepository;
            _taskExceptions = new List<Task>();
            _serviceBus = serviceBus;
        }

        public async Task<User> DetectFaceAsync(string face)
        {
            var user = new User
            {
                FaceBase64 = face,
                InProcessing = false
            };

            try
            {
                using (var streamFace = new MemoryStream(Convert.FromBase64String(face)))
                {
                    var faceResult = await _faceServiceClient.DetectAsync(streamFace);
                    var detectedFace = faceResult.FirstOrDefault();
                    
                    user.UserId = detectedFace.FaceId;
                    user.InProcessing = true;
                    _detectedUsers.Add(user);
                    _userRepository.InsertUser(user);

                    await _serviceBus.SendLogAsync($"User \"{user.UserId.ToString()}\" detectado");
                    Task.Factory.StartNew(() => FindSimilarsAsync());
                    
                    streamFace.Flush();
                    streamFace.Close();
                    return user;
                }
            }
            catch (Exception ex)
            {
                SendLog($"Erro ao detectar a face {ex}");
                return user;
            }
        }

        public async Task FindSimilarsAsync()
        {
            if (_lastTask != null && !_lastTask.IsCompleted)
                return;

            while (true)
            {
                    
                User user = null;
                User userRetrived = null;

                lock (_detectedUsers)
                {
                    user = _detectedUsers.FirstOrDefault();
                }

                if (user != null)
                {
                    var containsAnyFaceOnList = UpsertFaceListAndCheckIfContainsFaceAsync().Result;

                    if (containsAnyFaceOnList)
                    {
                        SendLog($"Buscando similaridades do user \"{user.UserId.ToString()}\"");
                        var similarsFaces = await _faceServiceClient.FindSimilarAsync(user.UserId, _faceListId, 20);

                        var referenceFace = similarsFaces.FirstOrDefault(x => x.Confidence >= 0.5);

                        if (referenceFace == null)
                        {
                            SendLog($"Novo User detectado - \"{user.UserId.ToString()}\" ");
                            user.PersistedId = await AddUserToFaceListAsync(user.FaceBase64);
                        }
                        else
                        {
                            userRetrived = await GetUserByPersistedId(referenceFace.PersistedFaceId);
                            SendLog($"Similaridade encontrada de User - \"{user.UserId.ToString()}\" para User já existente {userRetrived.UserId.ToString()} ");
                            user.GuidReference = userRetrived.UserId.ToString();
                            user.Restrictions = userRetrived.Restrictions;
                        }
                    }
                    else
                        user.PersistedId = await AddUserToFaceListAsync(user.FaceBase64);

                    _lastTask = _userRepository.UpdateUserAsync(user);
                    await _lastTask;

                    HandleException(_userService.UserRetrieved(userRetrived));
                    _detectedUsers.Remove(user);
                }
            }
        }
        
        private async Task<bool> UpsertFaceListAndCheckIfContainsFaceAsync()
        {
            var faceLists = await _faceServiceClient.ListFaceListsAsync();
            var faceList = faceLists.FirstOrDefault(_ => _.FaceListId == _faceListId);

            if (faceList == null)
            {
                await _faceServiceClient.CreateFaceListAsync(_faceListId, "geekburger-users-face", null);
                return false;
            }

            var faceListJustCreated = await _faceServiceClient.GetFaceListAsync(_faceListId);

            return faceListJustCreated.PersistedFaces.Any();
        }

        private async Task<Guid> AddUserToFaceListAsync(string face)
        {
            using (var streamFace = new MemoryStream(Convert.FromBase64String(face)))
            {
                var persistedFace = await _faceServiceClient.AddFaceToFaceListAsync(_faceListId, streamFace);
                return persistedFace.PersistedFaceId;
            }
        }

        private bool HandleException(Task currentTask)
        {
            if (!currentTask.IsCompletedSuccessfully && currentTask.Exception != null)
            {
                SendLog($"Task incompleta: {currentTask.Exception}");
                _taskExceptions.Add(currentTask);
                return false;
            }

            return true;
        }


        private async Task<User> GetUserByPersistedId(Guid persistedGuid) => await _userRepository.GetUser(x => x.PersistedId == persistedGuid);

        private async void SendLog(string message) => await _serviceBus.SendLogAsync(message);

    }
}
