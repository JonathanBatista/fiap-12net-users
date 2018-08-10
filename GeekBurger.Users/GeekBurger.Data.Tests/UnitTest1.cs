using GeekBurger.Users.Application.AzureServices.AzureConnections;
using GeekBurger.Users.Application.AzureServices.Services;
using GeekBurger.Users.Core.Domains;
using GeekBurger.Users.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GeekBurger.Data.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void InsertInterfaceTest()
        {
            var repo = new UserRepository();
            repo.InsertFace("hdfhdsafpuf8sdap9fup3jfç398fuU9OPJF93JUHÇOJH23ÇJ93JU");
        }

        [Fact]
        public void ServiceBusTest()
        {
            var userService = new UserService(new AzureServiceBus());
          

            var user =
           new User
           {
               UserId = Guid.NewGuid(),
               Restrictions = new List<UserRestriction>
                {
                    new UserRestriction
                    {
                        Ingredient = "soy",
                        UserId = 1
                    }
                }
           };
            
            userService.UserRetrieved(user);
         
        }
    }
}
