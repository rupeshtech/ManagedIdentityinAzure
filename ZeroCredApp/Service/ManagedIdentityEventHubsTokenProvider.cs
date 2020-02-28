using Microsoft.Azure.EventHubs;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeroCredApp.Service
{
    public class ManagedIdentityEventHubsTokenProvider : ITokenProvider
    {
        private readonly string _managedIdentityTenantId;

        public ManagedIdentityEventHubsTokenProvider(string managedIdentityTenantId)
        {
            _managedIdentityTenantId = managedIdentityTenantId;
        }

        public async Task<SecurityToken> GetTokenAsync(string appliesTo, TimeSpan timeout)
        {
            string accessToken = await GetAccessToken("https://eventhubs.azure.net/");
            return new JsonSecurityToken(accessToken, appliesTo);
        }

        private async Task<string> GetAccessToken(string resource)
        {
            var authProvider = new AzureServiceTokenProvider();
            return await authProvider.GetAccessTokenAsync(resource, _managedIdentityTenantId);
        }
    }
}
