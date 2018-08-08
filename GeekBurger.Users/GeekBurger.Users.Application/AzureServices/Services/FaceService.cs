﻿using GeekBurger.Users.Core.Configs;
using GeekBurger.Users.Core.Domains;
using Microsoft.ProjectOxford.Face;
using System;
using System.Collections;
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
        private List<User> _detectedUsers;
        private Task _lastTask;


        public FaceService(IUserService userService)
        {
            _faceListId = ConfigurationManager.Configuration["azureConfig:faceListId"];
            _faceServiceClient = new FaceServiceClient(ConfigurationManager.Configuration["azureConfig:key1"], ConfigurationManager.Configuration["azureConfig:endpoint"]);
            _detectedUsers = new List<User>();
            _userService = userService;
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

                    _lastTask = Task.Factory.StartNew(() => FindSimilarsAsync());

                    //salvar usuário
                    
                    streamFace.Flush();
                    streamFace.Close();
                    return user;
                }
            }
            catch (Exception ex)
            {
                //log

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
                        var similarsFaces = await _faceServiceClient.FindSimilarAsync(user.UserId, _faceListId, 20);

                        var referenceFace = similarsFaces.FirstOrDefault(x => x.Confidence >= 0.5);

                        if (referenceFace != null)
                            user.PersistedId = await AddUserToFaceListAsync(user.FaceBase64);
                        else
                        {
                            userRetrived = GetUserByPersistedId(referenceFace.PersistedFaceId);
                            user.GuidReference = userRetrived.UserId.ToString();
                        }
                    }
                    else
                        user.PersistedId = await AddUserToFaceListAsync(user.FaceBase64);


                    // update user


                    await _userService.UserRetrieved(userRetrived);
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



        private User GetUserByPersistedId(Guid persistedGuid)
        {
            return new User
            {
                Restrictions = new List<UserRestriction>(),
                UserId = Guid.NewGuid()
            };
        }

    }
}
