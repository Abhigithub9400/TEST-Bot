using MediAssist.Application.Abstract.Repositories;
using MediAssist.Application.Abstract.Services;
using MediAssist.Configurations.Logo;
using MediAssist.DbContext;
using MediAssist.Infrastructure.Abstract.Configurations;
using MediAssist.Infrastructure.HttpProvider.Services.Abstract;
using System.Text.Encodings.Web;
using static MediAssist.Configurations.GlobalEnums;

namespace MediAssist.Application.Services
{
    public class EmailManagementService : IEmailManagementService
    {
        #region PRIVATE FIELDS
        private readonly IAppSettings _appSettings;
        private readonly IMailService _mailService;
        private readonly IUserRepository _userRepository;
        private readonly IEmailRepository _emailRepository;
        #endregion

        #region CONSTRUCTOR
        public EmailManagementService(IAppSettings appSettings, IMailService mailService, IUserRepository userRepository, IEmailRepository emailRepository)
        {
            _appSettings = appSettings;
            _mailService = mailService;
            _userRepository = userRepository;
            _emailRepository = emailRepository;
        }
        #endregion

        #region PUBLIC METHODS

        public async Task<bool> SendResetPasswordEmail(string firstName, string email, string resetCode)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(resetCode))
                throw new ArgumentException("Invalid parameters for sending reset password email.");

            try
            {
                var emailTemplate = await _emailRepository.Get_EmailTemplate(EmailIdentifiers.ResetRequest);
                if (emailTemplate == null || string.IsNullOrWhiteSpace(emailTemplate.HTMLBody))
                    throw new InvalidOperationException("Reset password email template is missing or invalid.");

                var emailBody = ConstructResetPasswordEmailTemplate(firstName, resetCode, emailTemplate.HTMLBody);
                var subject = ResolveCommonVariablesInSubject(emailTemplate.Subject) ?? "Password Reset Request";

                return await _mailService.SendEmailToUserAsync(emailBody, email, subject);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while sending the reset password email to {email}. See inner exception for details.", ex);
            }
        }

        public async Task<bool> SendRequestDemoEmail(string name, string email, string phoneNumber, string requirements)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Name and email are required for sending demo request email.");

            try
            {
                var requestDateTime = DateTime.Now;
                var emailTemplate = await _emailRepository.Get_EmailTemplate(EmailIdentifiers.RequestDemo);
                if (emailTemplate == null || string.IsNullOrWhiteSpace(emailTemplate.HTMLBody))
                    throw new InvalidOperationException("Demo request email template is missing or invalid.");

                var emailBody = ConstructEmailTemplateToSupport(name, email, phoneNumber, requirements, emailTemplate.HTMLBody, requestDateTime);
                var subject = ResolveCommonVariablesInSubject(emailTemplate.Subject) ?? "Demo Request Submission";

                var isSuccess = await _mailService.SendEmailToSupportAsync(emailBody, subject);

                if (isSuccess)
                {
                    var requestLog = new DemoRequestLog
                    {
                        RequesterName = name,
                        EmailAddress = email,
                        PhoneNumber = phoneNumber,
                        Requirements = requirements,
                        CreatedDate = requestDateTime
                    };
                    _userRepository.LogDemoRequest(requestLog);
                }

                return isSuccess;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while processing demo request email to {email}: {ex.Message}", ex);
            }
        }

        public async Task<bool> SendGenericEnquiryEmail(string name, string email, string phoneNumber, string requirements)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Name and email are required for sending demo request email.");

            try
            {
                var requestDateTime = DateTime.Now;
                var emailTemplate = await _emailRepository.Get_EmailTemplate(EmailIdentifiers.GenericEnquiry);
                if (emailTemplate == null || string.IsNullOrWhiteSpace(emailTemplate.HTMLBody))
                    throw new InvalidOperationException("ContactUs request email template is missing or invalid.");

                var emailBody = ConstructEmailTemplateToSupport(name, email, phoneNumber, requirements, emailTemplate.HTMLBody, requestDateTime);
                var subject = ResolveCommonVariablesInSubject(emailTemplate.Subject) ?? "New Request Submission";

                var isSuccess = await _mailService.SendEmailToSupportAsync(emailBody, subject);

                if (isSuccess)
                {
                    await SendThankYouEmailForInquiry(name, email);
                }

                return isSuccess;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while processing demo request email to {email}: {ex.Message}", ex);
            }
        }

        public async Task<bool> ContactUsForSubscription(string name, string email, string phoneNumber, string additionalNotes, string selectedPlan)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(selectedPlan))
                throw new ArgumentException("Name, email, and selected plan are required for subscription inquiry.");

            try
            {
                var requestDateTime = DateTime.Now;
                var user = await _userRepository.GetUserByEmailAsync(email);

                var emailTemplate = user != null
                    ? await _emailRepository.Get_EmailTemplate(EmailIdentifiers.ContactUsForSubscriptionForRegisteredUserTemplate)
                    : await _emailRepository.Get_EmailTemplate(EmailIdentifiers.ContactUsForSubscriptionForGuestUserTemplate);

                if (emailTemplate == null || string.IsNullOrWhiteSpace(emailTemplate.HTMLBody))
                    throw new InvalidOperationException("Subscription inquiry email template is missing or invalid.");

                var emailBody = ConstructEmailTemplateToSupport(name, email, phoneNumber, additionalNotes, emailTemplate.HTMLBody, requestDateTime)
                                .Replace("{{PLAN NAME}}", HtmlEncoder.Default.Encode(selectedPlan));
                var subject = ResolveCommonVariablesInSubject(emailTemplate.Subject) ?? (user != null ? "Plan Inquiry from Registered User" : "New Paid Plan Inquiry");

                var isSuccess = await _mailService.SendEmailToSupportAsync(emailBody, subject);

                if (isSuccess)
                {
                    await SendThankYouEmailForInquiry(name, email);
                }

                return isSuccess;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while processing subscription inquiry to {email}: {ex.Message}", ex);
            }
        }

        public async Task<bool> SendThankYouEmailForInquiry(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Name and email are required for sending thank you email.");

            try
            {
                var emailTemplate = await _emailRepository.Get_EmailTemplate(EmailIdentifiers.ThankyouMailToUserOnPricingEnquiry);
                if (emailTemplate == null || string.IsNullOrWhiteSpace(emailTemplate.HTMLBody))
                    throw new InvalidOperationException("Thank you email template is missing or invalid.");

                var emailBody = ConstructEmailTemplateToUser(name, emailTemplate.HTMLBody);
                var subject = ResolveCommonVariablesInSubject(emailTemplate.Subject) ?? "Thank You for Your Inquiry!";

                return await _mailService.SendEmailToUserAsync(emailBody, email, subject);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while sending thank you email to {email}: {ex.Message}", ex);
            }
        }

        public async Task<bool> SendReportRequestEmail(string file, string email, string name, string hospitalName, string hospitalAddress, string reportName, string patientName, string doctorName, string consultationDate)
        {
            try
            {
                var emailTemplate = await _emailRepository.Get_EmailTemplate(EmailIdentifiers.ShareReport);
                if (emailTemplate == null || string.IsNullOrWhiteSpace(emailTemplate.HTMLBody))
                    throw new InvalidOperationException("Thank you email template is missing or invalid.");

                var emailBody = ConstructReportSendTemplate(email, name, hospitalName, hospitalAddress, reportName, doctorName, consultationDate, emailTemplate.HTMLBody);
                var emailSubject = ConstructReportSendSubjectTemplate(hospitalName, reportName, emailTemplate.Subject);

                bool success = await _mailService.SendEmailToUserAsync(emailBody, email, emailSubject, file, reportName, patientName);
                return success;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while processing your request: {ex.Message}", ex);
            }
        }
        #endregion

        #region PRIVATE METHODS

        private string ResolveCommonVariablesInSubject(string emailSubject)
        {
            if (string.IsNullOrWhiteSpace(emailSubject))
                throw new ArgumentException("Reset password email template cannot be null or empty.");

            return emailSubject.Replace("{{DOMAIN NAME}}", $"{_appSettings.MediAssistDomainName}");
        }

        private string ResolveCommonVariables(string bodyTemplate)
        {
            if (string.IsNullOrWhiteSpace(bodyTemplate))
                throw new ArgumentException("Reset password email template cannot be null or empty.");

            return bodyTemplate.Replace("{{DOMAIN URL}}", $"{_appSettings.MediAssistBaseUrl}")
               .Replace("{{MEDIASSIST LOGO}}", $"{_appSettings.MediAssistBaseUrl}/images/logo.png")
               .Replace("{{HEADER IMAGE}}", $"{_appSettings.MediAssistBaseUrl}/images/headerImage.png")
               .Replace("{{DOMAIN NAME}}", $"{_appSettings.MediAssistDomainName}")
               .Replace("{{POLICY PAGE URL}}", $"{_appSettings.MediAssistBaseUrl}/privacy-policy");
        }

        private string ConstructResetPasswordEmailTemplate(string firstName, string resetCode, string bodyTemplate)
        {
            if (string.IsNullOrWhiteSpace(bodyTemplate))
                throw new ArgumentException("Reset password email template cannot be null or empty.");

            var resetLink = $"{_appSettings.MediAssistBaseUrl}/reset-password?token=";

            bodyTemplate = ResolveCommonVariables(bodyTemplate);

            return bodyTemplate
                .Replace("{{USER FIRST NAME}}", firstName)
                .Replace("{{RESET CODE}}", resetCode)
                .Replace("{{RESET PASSWORD LINK}}", resetLink);
        }

        private string ConstructEmailTemplateToSupport(string name, string email, string phoneNumber, string additionalNotes, string bodyTemplate, DateTime requestDateTime)
        {
            if (string.IsNullOrWhiteSpace(bodyTemplate))
                throw new ArgumentException("Email body template cannot be null or empty.");

            var requirementsSection = !string.IsNullOrWhiteSpace(additionalNotes)
                ? $"<strong>Additional Notes / Requirements:</strong> {HtmlEncoder.Default.Encode(additionalNotes)}"
                : string.Empty;

            bodyTemplate = ResolveCommonVariables(bodyTemplate);

            return bodyTemplate
                .Replace("{{USER NAME}}", HtmlEncoder.Default.Encode(name))
                .Replace("{{USER EMAIL}}", HtmlEncoder.Default.Encode(email))
                .Replace("{{USER PHONENUMBER}}", HtmlEncoder.Default.Encode(phoneNumber))
                .Replace("{{REQUIREMENTS}}", requirementsSection)
                .Replace("{{REQUEST TIMESTAMP}}", requestDateTime.ToString("dd-MM-yyyy h tt", System.Globalization.CultureInfo.InvariantCulture) + " IST");
        }

        private string ConstructEmailTemplateToUser(string name, string bodyTemplate)
        {
            if (string.IsNullOrWhiteSpace(bodyTemplate))
                throw new ArgumentException("Email body template cannot be null or empty.");

            bodyTemplate = ResolveCommonVariables(bodyTemplate);

            return bodyTemplate.Replace("{{USER NAME}}", HtmlEncoder.Default.Encode(name));
        }

        private string ConstructReportSendTemplate(string email, string name, string hospitlName, string hospitalAddress, string reportName, string doctorName, string consultationDtae, string bodyTemplate)
        {
            bodyTemplate = ResolveCommonVariables(bodyTemplate);
            string body = bodyTemplate
                .Replace("{{RECIPIENTNAME}}", HtmlEncoder.Default.Encode(name))
                .Replace("{{EMAIL}}", HtmlEncoder.Default.Encode(email))
                .Replace("{{HOSPITALNAME}}", HtmlEncoder.Default.Encode(hospitlName))
                .Replace("{{HOSPITALADDRESS}}", HtmlEncoder.Default.Encode(hospitalAddress))
                .Replace("{{REPORTNAME}}", HtmlEncoder.Default.Encode(reportName))
                .Replace("{{DOCTORNAME}}", HtmlEncoder.Default.Encode(doctorName))
                .Replace("{{CONSULTATIONDATE}}", HtmlEncoder.Default.Encode(consultationDtae));

            return body;
        }

        private string ConstructReportSendSubjectTemplate(string hospitlName, string reportName, string subjectTemplate)
        {
            string subject = subjectTemplate
                .Replace("{{HOSPITALNAME}}", HtmlEncoder.Default.Encode(hospitlName))
                .Replace("{{REPORTNAME}}", HtmlEncoder.Default.Encode(reportName));

            return subject;
        }
        #endregion
    }
}
