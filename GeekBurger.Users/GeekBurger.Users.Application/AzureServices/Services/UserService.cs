using GeekBurger.Users.Contract;
using GeekBurger.Users.Core.Configs;
using GeekBurger.Users.Core.Domains;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;

namespace GeekBurger.Users.Application.AzureServices.Services
{
    public class UserService : IUserService
    {
        private readonly IServiceBus _serviceBus;

        public UserService(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        public void UserRetrieved(User user)
        {
            var message = GetMessage(new UserRetrieved()
            {
                AreRestrictionsSet = user.Restrictions.Any(),
                UserId = user.AzureGuid
            });

            _serviceBus.SendMessageAsync(ConfigurationManager.Configuration["appSettings:sbUserRetrived"], message);
        }

        private Message GetMessage(UserRetrieved userRetrieved)
        {
            var userSerialized = JsonConvert.SerializeObject(userRetrieved);
            var userByteArray = Encoding.UTF8.GetBytes(userSerialized);

            return new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = userByteArray
            };
        }
    }
}
