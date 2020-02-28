using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZeroCredApp.Service
{
    public class EventHubProcessor : IEventProcessor
    {
        //private readonly TelemetryClient _telemetryClient;
        private readonly IHubContext<EventHubMessageHub, IClientReceiver> _messageHub;

        public EventHubProcessor(IHubContext<EventHubMessageHub, IClientReceiver> messageHub)
        {
            _messageHub = messageHub;
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (EventData eventData in messages)
            {
                string data = Encoding.UTF8.GetString(
                    eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                await _messageHub.Clients.All.ReceiveMessage(data);
            }

            await context.CheckpointAsync();
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            return Task.CompletedTask;
        }

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            return Task.CompletedTask;
        }
    }
}
