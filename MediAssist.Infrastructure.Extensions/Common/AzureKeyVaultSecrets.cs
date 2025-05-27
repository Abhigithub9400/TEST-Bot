using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

namespace MediAssist.Infrastructure.Extensions.Common
{
    public static class AzureKeyVaultSecrets
    {
        private static SecretClient _secretClient;

        public static void Initialize(IConfiguration configuration)
        {
            var tenantId = configuration["AzureAd:TenantId"];
            var clientId = configuration["AzureAd:ClientId"];
            var clientSecret = configuration["AzureAd:ClientSecret"];
            var vaultUrl = configuration["AzureKeyVault:VaultUrl"];

            var credentials = new ClientSecretCredential(tenantId, clientId, clientSecret);
            _secretClient = new SecretClient(new Uri(vaultUrl), credentials);
        }

        public static void InitializeWithManagedIdentity()
        {
            var vaultUrl = Environment.GetEnvironmentVariable("VaultUri");
            if (string.IsNullOrEmpty(vaultUrl))
            {
                throw new InvalidOperationException("VaultUri environment variable is not set.");
            }

            var credential = new DefaultAzureCredential();
            _secretClient = new SecretClient(new Uri(vaultUrl), credential);
        }

        public static string GetSecret(string secretName)
        {
            if (_secretClient == null)
            {
                throw new InvalidOperationException("AzureKeyVaultSecrets is not initialized.");
            }

            KeyVaultSecret secret = _secretClient.GetSecret(secretName);
            return secret.Value;
        }
    }
}
