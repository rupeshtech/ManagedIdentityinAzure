using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroCredApp.Settings;

namespace ZeroCredApp.Service
{
    public class QueueListenerService : HostedService
    {
        private readonly IHubContext<QueueMessageHub, IClientReceiver> _messageHub;
        private readonly DemoSettings _settings;

        public QueueListenerService(
            IHubContext<QueueMessageHub, IClientReceiver> messageHub,
            IOptions<DemoSettings> demoSettings)
        {
            _messageHub = messageHub;
            _settings = demoSettings.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            string endpoint = _settings.ServiceBusNamespace + ".servicebus.windows.net";
            string queueName = _settings.ServiceBusQueueName;
            var tokenProvider = new ManagedIdentityServiceBusTokenProvider(_settings.ManagedIdentityTenantId);
            var queueClient = new QueueClient(endpoint, queueName,tokenProvider,TransportType.Amqp,ReceiveMode.PeekLock);

            try
            {
                var messageHandlerOptions = new MessageHandlerOptions(HandleException)
                {
                    AutoComplete = true
                };
                queueClient.RegisterMessageHandler(HandleMessage, messageHandlerOptions);
                await Task.Delay(-1, cancellationToken);
            }
            catch (UnauthorizedException e)
            {
                // Log and exit
            }
            finally
            {
                await queueClient.CloseAsync();
            }
        }

        private async Task HandleMessage(Message msg, CancellationToken ct)
        {
            string message = Encoding.UTF8.GetString(msg.Body);
            await _messageHub.Clients.All.ReceiveMessage(message);
        }

        private Task HandleException(ExceptionReceivedEventArgs errArgs)
        {
            return Task.CompletedTask;
        }
    }
}
