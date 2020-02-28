using Microsoft.AspNetCore.SignalR;

namespace ZeroCredApp.Service
{
    public class QueueMessageHub : Hub<IClientReceiver>
    {
    }
}
