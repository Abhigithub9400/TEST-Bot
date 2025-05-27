using MediAssist.Infrastructure.Abstract.Configurations;
using Microsoft.Extensions.Configuration;

namespace MediAssist.Configurations
{
    public class Appsettings(IConfiguration configuration) : IAppSettings
    {
        public string ApiKey => configuration["FireBase:ApiKey"] ?? string.Empty;

        public int ExpirationDays
        {
            get
            {
                var expirationDaysString = configuration["Authentication:DefaultExpirationDays"];
                return int.TryParse(expirationDaysString, out var expirationDays) ? expirationDays : 0; 
            }
        }
        public string Email => configuration["SMTP:Email"] ?? string.Empty;

        public string Password => configuration["SMTP:Password"] ?? string.Empty;

        public string Host => configuration["SMTP:Host"] ?? string.Empty;

        public int Port => int.Parse(configuration["SMTP:Port"] ?? string.Empty);

        public string BdeTeamEmail => configuration["EmailConfig:BdeTeamEmail"] ?? string.Empty;

        public string MediAssistSupportEmail => configuration["EmailConfig:MediAssistSupportEmail"] ?? string.Empty;

        public string MediAssistBaseUrl => configuration["MediAssistBaseUrl"] ?? string.Empty;

        public string MediAssistDomainName => configuration["MediAssistDomainName"] ?? string.Empty;

        public string Key => configuration["JwtToken:Key"] ?? string.Empty;

        public string Issuer => configuration["JwtToken:Issuer"] ?? string.Empty;

        public string Audience => configuration["JwtToken:Audience"] ?? string.Empty;

        public string  FHIRBaseUrl => configuration["FHIRBaseUrl"] ?? string.Empty;
    }
}
