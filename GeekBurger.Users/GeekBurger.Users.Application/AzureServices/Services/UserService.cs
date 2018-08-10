using GeekBurger.Users.Application.AzureServices.AzureConnections;
using GeekBurger.Users.Contract;
using GeekBurger.Users.Core.Configs;
using GeekBurger.Users.Core.Domains;
using GeekBurger.Users.Data;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.Users.Application.AzureServices.Services
{
    public class UserService : IUserService
    {
        private readonly IServiceBus _serviceBus;
        private readonly IUserRepository _userRepository;
        private AzureServiceBus azureServiceBus;

        public UserService(IServiceBus serviceBus, IUserRepository userRepository)
        {
            _serviceBus = serviceBus;
            _userRepository = userRepository;
        }

        public async void SaveUserRestriction(Guid userId, List<string> restrictions, string other)
        {
            var user = _userRepository.GetUser(u => u.UserId == userId).Result;
            var outros = other.Split(",");
            restrictions.AddRange(outros);
            await _serviceBus.SendLogAsync($"Salvando as restrições do user \"{userId.ToString()}\"  !!!");
            foreach (var restriction in restrictions)
            {
                var userRestriction = new UserRestriction(user);
                userRestriction.Ingredient = restriction.Trim();
                userRestriction.UserId = user.Id;

                _userRepository.InserFoodRestriction(userRestriction);
            }

        }

        public async Task UserRetrieved(User user)
        {
            var message = GetMessage(new UserRetrieved()
            {
                AreRestrictionsSet = user.Restrictions.Any(),
                UserId = user.UserId.ToString()
            });

            await _serviceBus.SendLogAsync($"Enviando User \"{user.UserId.ToString()}\" para a fila");
            await _serviceBus.SendMessageAsync(ConfigurationManager.Configuration["appSettings:sbUserRetrived"], message);

        }

        private Message GetMessage(UserRetrieved userRetrieved)
        {
            var userSerialized = JsonConvert.SerializeObject(userRetrieved);
            var userByteArray = Encoding.UTF8.GetBytes(userSerialized);

            return new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = userByteArray,
                Label = "Users"
            };
        }
    }
}
