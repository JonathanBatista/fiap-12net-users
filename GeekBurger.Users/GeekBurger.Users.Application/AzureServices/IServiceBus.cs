using Microsoft.Azure.ServiceBus;

namespace GeekBurger.Users.Application.AzureServices
{
    public interface IServiceBus
    {
        void SendMessageAsync(string topicName, Message message);
    }
}
