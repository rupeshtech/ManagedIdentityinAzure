using Microsoft.AspNetCore.SignalR;

namespace ZeroCredApp.Service 
{ 
    public class EventHubMessageHub : Hub<IClientReceiver>
    {
    }
}
