using MediAssist.Infrastructure.Abstract.Configurations;
using MediAssist.Infrastructure.HttpProvider.Services.Abstract;
using System.Net;
using System.Net.Mail;

namespace MediAssist.Infrastructure.HttpProvider.Services
{
    public class Mailservice(IAppSettings appsettings) : IMailService
    {
        public async Task<bool> SendEmailToUserAsync(string body, string recipient, string subject, string? file = null, string? reportName = null, string? patientName = null)
        {
            string mail = appsettings.Email;
            string password = appsettings.Password;
            SmtpClient client = new(appsettings.Host, appsettings.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };
            MailMessage mailMessage = new(from: mail,
                to: recipient,
                subject: subject,
                body: body);

            mailMessage.IsBodyHtml = true;

            if (!string.IsNullOrEmpty(file))
            {
                try
                {
                    byte[] fileBytes = Convert.FromBase64String(file);
                    MemoryStream stream = new(fileBytes);
                    string filename = $"{patientName}_{reportName}.pdf";
                    Attachment attachment = new(stream, filename, "application/pdf");
                    mailMessage.Attachments.Add(attachment);
                }
                catch (FormatException ex)
                {
                    throw new Exception("Invalid Base64 string provided for file attachment.", ex);
                }
            }

            await client.SendMailAsync(mailMessage);

            return true;
        }

        public async Task<bool> SendEmailToSupportAsync(string body, string subject)
        {
            try
            {
                string mail = appsettings.Email;
                string mediAssistSupport = appsettings.MediAssistSupportEmail;
                string password = appsettings.Password;

                using SmtpClient client = new(appsettings.Host, appsettings.Port)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };

                MailMessage mailMessage = new(
                    from: mail,
                    to: mediAssistSupport,
                    subject: subject,
                    body: body
                );
                mailMessage.IsBodyHtml = true;

                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
