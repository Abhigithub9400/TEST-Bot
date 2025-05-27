using Azure.Security.KeyVault.Secrets;
using MediAssist.Infrastructure.HttpProvider.Services.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

public class AzureKeyVaultService : IAzureKeyVaultService
{
    private readonly SecretClient _secretClient;
    private readonly ILogger<AzureKeyVaultService> _logger;
    private readonly ConcurrentDictionary<string, string> _secretCache = new();

    // Optional: still know if you're in Production
    private readonly bool _isProduction;

    public AzureKeyVaultService(
        SecretClient secretClient,
        IConfiguration configuration,
        ILogger<AzureKeyVaultService> logger)
    {
        _secretClient = secretClient;
        _logger = logger;
        _isProduction = configuration["ASPNETCORE_ENVIRONMENT"] == "Production";

        _logger.LogInformation("AzureKeyVaultService initialized (Production={IsProd})",
            _isProduction);
    }

    public async Task<string?> GetSecretAsync(string secretName)
    {
        if (_secretCache.TryGetValue(secretName, out var cached))
            return cached;

        try
        {
            var secret = await _secretClient.GetSecretAsync(secretName);
            _secretCache[secretName] = secret.Value.Value;
            return secret.Value.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch secret '{SecretName}' from Key Vault", secretName);

            if (_isProduction)
            {
                _logger.LogWarning("Returning fallback for '{SecretName}' in production", secretName);
                return secretName switch
                {
                    "Encryption-Key" => "A8D3F1C5E9B74A61C2D8F3A0B7E4C9D5",
                    "Encryption-IV" => "F1C5E9B74A61C2D8",
                    _ => string.Empty
                };
            }

            throw;
        }
    }
}
