using GeekBurger.Users.Application.AzureServices.Configs;
using GeekBurger.Users.Core.Configs;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GeekBurger.Users.Application.AzureServices.AzureConnections
{
    public class AzureServiceBus : IServiceBus
    {
        private List<Message> _messages;
        private List<Task> _taskExceptions;
        private Task _lastTask;

        public AzureServiceBus()
        {
            _messages = new List<Message>();
            _taskExceptions = new List<Task>();
        }

        public async void SendMessageAsync(string topicName, Message message)
        {
            _messages.Add(message);
            if (_lastTask != null && !_lastTask.IsCompleted)
                return;

            var connectionString = ConfigurationManager.Configuration["connectionStrings:serviceBusConnectionString"];


            var topicClient = new TopicClient(connectionString, topicName);

            if (topicClient != null)
            {
                _lastTask = SendAsync(topicClient);
                await _lastTask;
                var closeTask = topicClient.CloseAsync();
                await closeTask;
                HandleException(closeTask);
            }
        }

        private void CreateTopic(string topicName)
        {
            var config = JsonConvert.DeserializeObject<ServiceBusConfiguration>(ConfigurationManager.Configuration["serviceBus"]);
            var credentials = SdkContext.AzureCredentialsFactory
                                            .FromServicePrincipal(config.ClientId,
                                            config.ClientSecret,
                                            config.TenantId,
                                            AzureEnvironment.AzureGlobalCloud);

            var serviceBusManager = ServiceBusManager.Authenticate(credentials, config.SubscriptionId);

            var serviceBusNamespace = serviceBusManager.Namespaces.GetByResourceGroup(config.ResourceGroup, config.NamespaceName);
            
            if (!serviceBusNamespace.Topics.List().Any(topic => topic.Name.Equals(topicName, StringComparison.InvariantCultureIgnoreCase)))
                serviceBusNamespace.Topics.Define(topicName).WithSizeInMB(1024).Create();
        }


        private bool HandleException(Task currentTask)
        {
            if (!currentTask.IsCompletedSuccessfully && currentTask.Exception != null)
            {
                _taskExceptions.Add(currentTask);
                // log
                return true;
            }
            
            return true;
        }

        private async Task SendAsync(ISenderClient queueClient)
        {
            int tries = 0;
            Message message;
            while (true)
            {
                if (_messages.Count <= 0)
                    break;
                lock (_messages)
                {
                    message = _messages.FirstOrDefault();
                }
                var sendTask = queueClient.SendAsync(message);
                await sendTask;
                var success = HandleException(sendTask);
                if (!success)
                    Thread.Sleep(10000 * (tries < 60 ? tries++ : tries));
                else
                    _messages.Remove(message);
            }            
        }
    }


    
}
