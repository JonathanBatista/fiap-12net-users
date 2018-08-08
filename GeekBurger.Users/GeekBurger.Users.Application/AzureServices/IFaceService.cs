using GeekBurger.Users.Core.Domains;
using System.Threading.Tasks;

namespace GeekBurger.Users.Application.AzureServices
{
    public interface IFaceService
    {
        Task<User> DetectFaceAsync(string face);

        //findSimilars
        
        //armazenar dados
    }
}
