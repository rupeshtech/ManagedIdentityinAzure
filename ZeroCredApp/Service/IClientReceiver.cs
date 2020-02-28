using System.Threading.Tasks;

namespace ZeroCredApp.Service
{
    public interface IClientReceiver
    {
        Task ReceiveMessage(string message);
    }
}
