using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.DataLake.Store;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZeroCredApp.Data;
using ZeroCredApp.Models;
using ZeroCredApp.Settings;

namespace ZeroCredApp.Service
{
    public class DemoService : IDemoService
    {
        private readonly DemoSettings _settings;
        private readonly BookShopDBContext _dbContext;

        public DemoService(
            IOptionsSnapshot<DemoSettings> settings,
            BookShopDBContext dbContext)
        {
            _settings = settings.Value;
            _dbContext = dbContext;
        }

        public async Task<List<Book>> AccessSqlDatabase()
        {
            List<Book> books = _dbContext.Books.ToList();
            return books;
        }

        public async Task<StorageViewModel> AccessStorage()
        {
            var blobServiceClient = new BlobServiceClient(new Uri($"https://{_settings.StorageAccountName}.blob.core.windows.net"),
                new ManagedIdentityStorageTokenCredential(_settings.ManagedIdentityTenantId));
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_settings.StorageContainerName);
            BlobClient blobClient = containerClient.GetBlobClient(_settings.StorageBlobName);
            Response<BlobDownloadInfo> response = await blobClient.DownloadAsync();

            using (var reader = new StreamReader(response.Value.Content))
            {
                string content = await reader.ReadToEndAsync();
                return new StorageViewModel
                {
                    FileContent = content
                };
            }
        }

        public async Task SendEventHubsMessage(string message)
        {
            string hubNamespace = _settings.EventHubNamespace;
            var endpoint = new Uri($"sb://{hubNamespace}.servicebus.windows.net/");
            string hubName = _settings.EventHubName;
            var client = EventHubClient.CreateWithTokenProvider(
                endpoint,
                hubName,
                new ManagedIdentityEventHubsTokenProvider(_settings.ManagedIdentityTenantId));
            byte[] bytes = Encoding.UTF8.GetBytes($"{message} ({DateTime.UtcNow:HH:mm:ss})");
            await client.SendAsync(new EventData(bytes));
        }

        public async Task SendServiceBusQueueMessage(string messageString)
        {
            string endpoint = _settings.ServiceBusNamespace + ".servicebus.windows.net";
            string queueName = _settings.ServiceBusQueueName;

            var tokenProvider = new ManagedIdentityServiceBusTokenProvider(_settings.ManagedIdentityTenantId);
            var queueClient = new QueueClient(endpoint, queueName,tokenProvider);

            var message = new Microsoft.Azure.ServiceBus.Message(
                Encoding.UTF8.GetBytes($"{messageString} ({DateTime.UtcNow:HH:mm:ss})"));
            await queueClient.SendAsync(message);
        }

        public async Task<DataLakeViewModel> AccessDataLake()
        {
            string token = await GetAccessToken("https://datalake.azure.net/");
            string accountFqdn = $"{_settings.DataLakeStoreName}.azuredatalakestore.net";
            var client = AdlsClient.CreateClient(accountFqdn, $"Bearer {token}");

            string filename = _settings.DataLakeFileName;
            string content = null;
            using (var reader = new StreamReader(await client.GetReadStreamAsync(filename)))
            {
                content = await reader.ReadToEndAsync();
            }

            return new DataLakeViewModel
            {
                FileContent = content
            };
        }

        private async Task<string> GetAccessToken(string resource, string tokenProviderConnectionString = null)
        {
            var authProvider = new AzureServiceTokenProvider(tokenProviderConnectionString);
            string tenantId = _settings.ManagedIdentityTenantId;

            if (tenantId != null && tenantId.Length == 0)
            {
                tenantId = null;
            }

            return await authProvider.GetAccessTokenAsync(resource, tenantId);
        }
    }
}
