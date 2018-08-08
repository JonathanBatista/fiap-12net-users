using GeekBurger.Users.Core.Domains;
using System.Threading.Tasks;

namespace GeekBurger.Users.Application.AzureServices
{
    public interface IUserService
    {
        Task UserRetrieved(User user);
    }
}
