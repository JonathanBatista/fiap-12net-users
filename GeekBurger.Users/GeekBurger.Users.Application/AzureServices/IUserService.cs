using GeekBurger.Users.Core.Domains;

namespace GeekBurger.Users.Application.AzureServices
{
    public interface IUserService
    {
        void UserRetrieved(User user);
    }
}
