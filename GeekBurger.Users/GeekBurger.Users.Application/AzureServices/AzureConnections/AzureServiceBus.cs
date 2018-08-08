using GeekBurger.Users.Application.AzureServices.Configs;
using GeekBurger.Users.Core.Configs;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GeekBurger.Users.Application.AzureServices.AzureConnections
{
    public class AzureServiceBus : IServiceBus
    {
        private readonly IServiceBusNamespace _serviceBusNamespace;
        private List<Message> _messages;
        private List<Task> _taskExceptions;
        private Task _lastTask;

        public AzureServiceBus()
        {
            _messages = new List<Message>();
            _taskExceptions = new List<Task>();

            var config = ConfigurationManager.Configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();
            var credentials = SdkContext.AzureCredentialsFactory
                                            .FromServicePrincipal(config.ClientId,
                                            config.ClientSecret,
                                            config.TenantId,
                                            AzureEnvironment.AzureGlobalCloud);

            var serviceBusManager = ServiceBusManager.Authenticate(credentials, config.SubscriptionId);

            _serviceBusNamespace = serviceBusManager.Namespaces.GetByResourceGroup(config.ResourceGroup, config.NamespaceName);
        }

        public async void SendMessageAsync(string topicName, Message message)
        {
            try
            {
                _messages.Add(message);
                if (_lastTask != null && !_lastTask.IsCompleted)
                    return;

                var connectionString = ConfigurationManager.Configuration["connectionStrings:serviceBusConnectionString"];

                CreateTopicIfNotExists(topicName);

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
            catch (Exception)
            {
                throw;
            }
            
        }

        private void CreateTopicIfNotExists(string topicName)
        {
            if (!_serviceBusNamespace.Topics.List().Any(topic => topic.Name.Equals(topicName, StringComparison.InvariantCultureIgnoreCase)))
                _serviceBusNamespace.Topics.Define(topicName).WithSizeInMB(1024).Create();
        }


        private bool HandleException(Task currentTask)
        {
            if (!currentTask.IsCompletedSuccessfully && currentTask.Exception != null)
            {
                _taskExceptions.Add(currentTask);
                return false;
            }
            
            return true;
        }

        private async Task SendAsync(ISenderClient topicClient)
        {
            int tries = 0;
            Message message;
            try
            {
                while (true)
                {
                    if (_messages.Count <= 0)
                        break;
                    lock (_messages)
                    {
                        message = _messages.FirstOrDefault();
                    }
                    var sendTask = topicClient.SendAsync(message);
                    await sendTask;
                    var success = HandleException(sendTask);
                    if (!success)
                        Thread.Sleep(10000 * (tries < 60 ? tries++ : tries));
                    else
                        _messages.Remove(message);
                }
            }
            catch (Exception)
            {
                throw;
            }          
        }
    }


    
}
