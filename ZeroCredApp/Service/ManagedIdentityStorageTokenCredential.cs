using Azure.Core;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ZeroCredApp.Service
{
    public class ManagedIdentityStorageTokenCredential : TokenCredential
    {
        private const string Resource = "https://storage.azure.com/";
        private readonly string _managedIdentityTenantId;
        public ManagedIdentityStorageTokenCredential(string managedIdentityTenantId)
        {
            _managedIdentityTenantId = managedIdentityTenantId;
        }
        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            return GetTokenAsync(requestContext, cancellationToken).GetAwaiter().GetResult();
        }

        public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            var authProvider = new AzureServiceTokenProvider();
            AppAuthenticationResult result = await authProvider.GetAuthenticationResultAsync(Resource, _managedIdentityTenantId);
            return new AccessToken(result.AccessToken, result.ExpiresOn);
        }
    }
}
