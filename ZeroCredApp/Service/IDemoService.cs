using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroCredApp.Data;
using ZeroCredApp.Models;

namespace ZeroCredApp.Service
{
    public interface IDemoService
    {
        Task<StorageViewModel> AccessStorage();
        Task<List<Book>> AccessSqlDatabase();
        Task SendServiceBusQueueMessage(string message);
        Task SendEventHubsMessage(string message);
        Task<DataLakeViewModel> AccessDataLake();
    }
}
