using GeekBurger.Users.Core.Domains;

namespace GeekBurger.Users.Application.AzureServices
{
    public interface IFaceService
    {
        User DetectFace(byte[] face);

        //findSimilars
        
        //armazenar dados
    }
}
