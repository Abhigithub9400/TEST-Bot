namespace MediAssist.Infrastructure.HttpProvider.Services.Abstract
{
    public interface IAzureKeyVaultService
    {
        Task<string> GetSecretAsync(string secretName);
    }
}
