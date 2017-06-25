using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyVaultTest1.Helpers
{
    public class VaultHelper
    {
        private string vaultURL = string.Empty;
        private string clientId = string.Empty;
        private string clientSecret = string.Empty;

        private KeyVaultClient kvc = null;

        private const string ALGORITHM = "RS256";

        public VaultHelper(string vaultURL, string clientId, string clientSecret)
        {
            this.vaultURL = vaultURL;
            this.clientId = clientId;
            this.clientSecret = clientSecret;

            kvc = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetToken));
        }

        public async Task<string> GetSecret(string secret)
        {
            var result = await kvc.GetSecretAsync(vaultURL, secret);
            return result.Value;
        }

        private async Task<string> GetToken(string authority, string resource, string scope)
        {
            new { authority, resource, scope }.CheckNotNull();

            var clientCredentials = new ClientCredential(clientId, clientSecret);
            var authenticationContext = new AuthenticationContext(authority);
            var result = await authenticationContext.AcquireTokenAsync(resource, clientCredentials);
            return result.AccessToken;
        }     
        
        public async Task<string> WrapKey(string key, byte[] unwrappedKey)
        {
            var wrappedKey = await KeyVaultClientExtensions.WrapKeyAsync(kvc, key, ALGORITHM, unwrappedKey);
            return wrappedKey.Kid;
        }


        public async Task<string> UnwrapKey(string key, byte[] wrappedKey)
        { 
            var unwrappedKey = await KeyVaultClientExtensions.UnwrapKeyAsync(kvc, key, ALGORITHM, wrappedKey);
            return unwrappedKey.Kid;
        }

    }
}
