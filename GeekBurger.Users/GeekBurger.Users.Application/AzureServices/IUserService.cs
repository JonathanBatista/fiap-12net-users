using GeekBurger.Users.Core.Domains;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeekBurger.Users.Application.AzureServices
{
    public interface IUserService
    {
        Task UserRetrieved(User user);

        Task SaveUserRestriction(Guid userId, List<string> restrictions, string other);
    }
}
