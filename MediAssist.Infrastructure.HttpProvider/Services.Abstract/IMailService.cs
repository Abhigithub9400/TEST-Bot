namespace MediAssist.Infrastructure.HttpProvider.Services.Abstract;

public interface IMailService
{
    Task<bool> SendEmailToUserAsync(string body, string recipient, string subject, string? file = null, string? reportName = null, string? patientName = null);

    Task<bool> SendEmailToSupportAsync(string body, string subject);
}
