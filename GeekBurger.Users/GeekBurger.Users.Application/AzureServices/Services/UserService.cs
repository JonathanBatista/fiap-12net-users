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

        public UserService(IServiceBus serviceBus, IUserRepository userRepository)
        {
            _serviceBus = serviceBus;
            _userRepository = userRepository;
        }

        public void SaveUserRestriction(Guid userId, List<string> restrictions, string other)
        {
            var user = _userRepository.GetUser(u => u.UserId == userId).Result;
            var outros = other.Split(",");
            restrictions.AddRange(outros);

            foreach(var restriction in restrictions)
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
            await _serviceBus.SendMessageAsync("log", message);
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
