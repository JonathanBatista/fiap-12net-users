using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;

namespace GeekBurger.Users.Application.AzureServices
{
    public interface IServiceBus
    {
        Task SendMessageAsync(string topicName, Message message);

        Task SendLogAsync(string logMessage);
    }
}
