namespace MediAssist.Infrastructure.Abstract.Configurations
{
    public interface IAppSettings
    {
        string ApiKey { get; }

        int ExpirationDays { get; }

        string Email { get; }

        string Password { get; }

        string Host { get; }

        int Port { get; }

        string BdeTeamEmail { get; }

        string MediAssistSupportEmail { get; }

        string MediAssistBaseUrl { get; }

        string MediAssistDomainName { get; }

        string Key { get; }

        string Issuer { get; }

        string Audience { get; }

        string FHIRBaseUrl { get; }
    }
}