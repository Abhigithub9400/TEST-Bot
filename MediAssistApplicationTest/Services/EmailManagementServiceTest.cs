using Auth0.ManagementApi.Models;
using MediAssist.Application.Abstract.Repositories;
using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Services;
using MediAssist.DbContext;
using MediAssist.Infrastructure.Abstract.Configurations;
using MediAssist.Infrastructure.HttpProvider.Services.Abstract;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using static MediAssist.Configurations.GlobalEnums;

namespace MediAssistApplicationTest.Services
{
    [TestFixture]
    public class EmailManagementServiceTest
    {
        #region PRIVATE INSTANCE FIELD

        private Mock<IAppSettings> _mockAppSettings;
        private Mock<IMailService> _mockMailService;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IEmailRepository> _mockEmailRepository;
        private EmailManagementService _service;
        private MethodInfo _resolveCommonVariablesInSubjectMethod;
        private MethodInfo _resolveCommonVariablesMethod;
        private MethodInfo _constructResetPasswordEmailTemplateMethod;
        private MethodInfo _constructEmailTemplateToSupportMethod;

        #endregion

        #region TEST SETUP

        [SetUp]
        public void SetUp()
        {
            _mockAppSettings = new Mock<IAppSettings>();
            _mockMailService = new Mock<IMailService>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockEmailRepository = new Mock<IEmailRepository>();

            _service = new EmailManagementService(
                _mockAppSettings.Object,
                _mockMailService.Object,
                _mockUserRepository.Object,
                _mockEmailRepository.Object);

            _mockAppSettings
            .Setup(x => x.MediAssistDomainName)
            .Returns("test.mediassist.com");

            _mockAppSettings
           .Setup(x => x.MediAssistBaseUrl)
           .Returns("https://test.mediassist.com");

            _resolveCommonVariablesInSubjectMethod = typeof(EmailManagementService)
         .GetMethod("ResolveCommonVariablesInSubject", BindingFlags.NonPublic | BindingFlags.Instance)
         ?? throw new InvalidOperationException("Method 'ResolveCommonVariablesInSubject' not found.");

            _resolveCommonVariablesMethod = typeof(EmailManagementService)
                .GetMethod("ResolveCommonVariables", BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw new InvalidOperationException("Method 'ResolveCommonVariables' not found.");

            _constructResetPasswordEmailTemplateMethod = typeof(EmailManagementService)
                .GetMethod("ConstructResetPasswordEmailTemplate", BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw new InvalidOperationException("Method 'ConstructResetPasswordEmailTemplate' not found.");

            _constructEmailTemplateToSupportMethod = typeof(EmailManagementService)
                .GetMethod("ConstructEmailTemplateToSupport", BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw new InvalidOperationException("Method 'ConstructEmailTemplateToSupport' not found.");
        }


        #endregion

        #region SENDRESETPASSWORDEMAIL

        [Test]
        public void ConstructEmailTemplateToSupport_WithValidParameters_ReplacesAllPlaceholders()
        {
            // Arrange
            string name = "John Doe";
            string email = "john.doe@example.com";
            string phoneNumber = "1234567890";
            string additionalNotes = "Test requirement";
            DateTime requestDateTime = new DateTime(2024, 2, 21, 14, 30, 0); 
            string bodyTemplate = @"
            Name: {{USER NAME}}
            Email: {{USER EMAIL}}
            Phone: {{USER PHONENUMBER}}
            Requirements: {{REQUIREMENTS}}
            Time: {{REQUEST TIMESTAMP}}
            Domain: {{DOMAIN URL}}";

            // Act
            string result = (string)_constructEmailTemplateToSupportMethod.Invoke(
                _service,
                new object[] { name, email, phoneNumber, additionalNotes, bodyTemplate, requestDateTime });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Contains.Substring(HtmlEncoder.Default.Encode(name)));
                Assert.That(result, Contains.Substring(HtmlEncoder.Default.Encode(email)));
                Assert.That(result, Contains.Substring(HtmlEncoder.Default.Encode(phoneNumber)));
                Assert.That(result, Contains.Substring("<strong>Additional Notes / Requirements:</strong>"));
                Assert.That(result, Contains.Substring(HtmlEncoder.Default.Encode(additionalNotes)));
                Assert.That(result, Contains.Substring("21-02-2024 2 PM IST"));
                Assert.That(result, Contains.Substring("https://test.mediassist.com"));
            });
        }

        [Test]
        public void ConstructEmailTemplateToSupport_WithHtmlInUserInput_EncodesHtml()
        {
            // Arrange
            string name = "<script>alert('xss')</script>";
            string email = "john@example.com";
            string phoneNumber = "1234567890";
            string additionalNotes = "<b>bold text</b>";
            DateTime requestDateTime = new DateTime(2024, 2, 21);
            string bodyTemplate = "Name: {{USER NAME}}, Notes: {{REQUIREMENTS}}";

            // Act
            string result = (string)_constructEmailTemplateToSupportMethod.Invoke(
                _service,
                new object[] { name, email, phoneNumber, additionalNotes, bodyTemplate, requestDateTime });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Contains.Substring(HtmlEncoder.Default.Encode(name)));
                Assert.That(result, Contains.Substring(HtmlEncoder.Default.Encode(additionalNotes)));
                Assert.That(result, Does.Not.Contain("<script>"));
                Assert.That(result, Does.Not.Contain("<b>"));
            });
        }

        [Test]
        public void ConstructEmailTemplateToSupport_WithoutAdditionalNotes_OmitsRequirementsSection()
        {
            // Arrange
            string name = "John Doe";
            string email = "john@example.com";
            string phoneNumber = "1234567890";
            string additionalNotes = "";
            DateTime requestDateTime = new DateTime(2024, 2, 21);
            string bodyTemplate = "Requirements: {{REQUIREMENTS}}";

            // Act
            string result = (string)_constructEmailTemplateToSupportMethod.Invoke(
                _service,
                new object[] { name, email, phoneNumber, additionalNotes, bodyTemplate, requestDateTime });

            // Assert
            Assert.That(result, Does.Not.Contain("Additional Notes / Requirements"));
            Assert.That(result, Does.Not.Contain("<strong>"));
        }

        [Test]
        public void ConstructEmailTemplateToSupport_WithNullTemplate_ThrowsArgumentException()
        {
            // Arrange
            string name = "John Doe";
            string email = "john@example.com";
            string phoneNumber = "1234567890";
            string additionalNotes = "Test";
            DateTime requestDateTime = new DateTime(2024, 2, 21);
            string bodyTemplate = null;

            // Act & Assert
            var exception = Assert.Throws<TargetInvocationException>(() =>
                _constructEmailTemplateToSupportMethod.Invoke(
                    _service,
                    new object[] { name, email, phoneNumber, additionalNotes, bodyTemplate, requestDateTime }));

            Assert.That(exception.InnerException, Is.TypeOf<ArgumentException>());
            Assert.That(exception.InnerException.Message,
                Is.EqualTo("Email body template cannot be null or empty."));
        }

        [Test]
        public void ConstructEmailTemplateToSupport_WithDifferentTimeZones_FormatsTimeCorrectly()
        {
            // Arrange
            string name = "John Doe";
            string email = "john@example.com";
            string phoneNumber = "1234567890";
            string additionalNotes = "Test";
            string bodyTemplate = "Time: {{REQUEST TIMESTAMP}}";

            // Test different times
            var testTimes = new[]
            {
            new DateTime(2024, 2, 21, 0, 0, 0),  
            new DateTime(2024, 2, 21, 12, 0, 0), 
            new DateTime(2024, 2, 21, 23, 59, 59) 
        };

            foreach (var dateTime in testTimes)
            {
                // Act
                string result = (string)_constructEmailTemplateToSupportMethod.Invoke(
                    _service,
                    new object[] { name, email, phoneNumber, additionalNotes, bodyTemplate, dateTime });

                // Assert
                string expectedTimeFormat = dateTime.ToString("dd-MM-yyyy h tt", System.Globalization.CultureInfo.InvariantCulture) + " IST";
                Assert.That(result, Contains.Substring(expectedTimeFormat));
            }
        }


        [Test]
        public void ResolveCommonVariables_WithAllPlaceholders_ReplacesAllCorrectly()
        {
            // Arrange
            string bodyTemplate = @"
            Domain URL: {{DOMAIN URL}}
            Logo: {{MEDIASSIST LOGO}}
            Header: {{HEADER IMAGE}}
            Domain Name: {{DOMAIN NAME}}
            Policy: {{POLICY PAGE URL}}";

            string expected = @"
            Domain URL: https://test.mediassist.com
            Logo: https://test.mediassist.com/images/logo.png
            Header: https://test.mediassist.com/images/headerImage.png
            Domain Name: test.mediassist.com
            Policy: https://test.mediassist.com/privacy-policy";

            // Act
            string result = (string)_resolveCommonVariablesMethod.Invoke(
                _service,
                new object[] { bodyTemplate });

            // Assert
            Assert.That(result, Is.EqualTo(expected));
            _mockAppSettings.Verify(x => x.MediAssistBaseUrl, Times.Exactly(4));
            _mockAppSettings.Verify(x => x.MediAssistDomainName, Times.Once);
        }

        [Test]
        public void ResolveCommonVariables_WithNullTemplate_ThrowsArgumentException()
        {
            // Arrange
            string bodyTemplate = null;

            // Act & Assert
            var exception = Assert.Throws<TargetInvocationException>(() =>
                _resolveCommonVariablesMethod.Invoke(
                    _service,
                    new object[] { bodyTemplate }));

            Assert.That(exception.InnerException, Is.TypeOf<ArgumentException>());
            Assert.That(exception.InnerException.Message,
                Is.EqualTo("Reset password email template cannot be null or empty."));
        }

        [Test]
        public void ResolveCommonVariables_WithEmptyTemplate_ThrowsArgumentException()
        {
            // Arrange
            string bodyTemplate = string.Empty;

            // Act & Assert
            var exception = Assert.Throws<TargetInvocationException>(() =>
                _resolveCommonVariablesMethod.Invoke(
                    _service,
                    new object[] { bodyTemplate }));

            Assert.That(exception.InnerException, Is.TypeOf<ArgumentException>());
        }

        [Test]
        public void ResolveCommonVariables_WithWhitespaceTemplate_ThrowsArgumentException()
        {
            // Arrange
            string bodyTemplate = "   ";

            // Act & Assert
            var exception = Assert.Throws<TargetInvocationException>(() =>
                _resolveCommonVariablesMethod.Invoke(
                    _service,
                    new object[] { bodyTemplate }));

            Assert.That(exception.InnerException, Is.TypeOf<ArgumentException>());
        }

        [Test]
        public void ResolveCommonVariables_WithNoPlaceholders_ReturnsUnchangedTemplate()
        {
            // Arrange
            string bodyTemplate = "This is a template without any placeholders";

            // Act
            string result = (string)_resolveCommonVariablesMethod.Invoke(
                _service,
                new object[] { bodyTemplate });

            // Assert
            Assert.That(result, Is.EqualTo(bodyTemplate));
        }

        [Test]
        public void ResolveCommonVariables_WithMultipleInstancesOfSamePlaceholder_ReplacesAll()
        {
            // Arrange
            string bodyTemplate = "{{DOMAIN URL}} and another {{DOMAIN URL}}";
            string expected = "https://test.mediassist.com and another https://test.mediassist.com";

            // Act
            string result = (string)_resolveCommonVariablesMethod.Invoke(
                _service,
                new object[] { bodyTemplate });

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ResolveCommonVariables_WithDifferentBaseUrl_UsesNewUrl()
        {
            // Arrange
            string newBaseUrl = "https://new.mediassist.com";
            _mockAppSettings.Setup(x => x.MediAssistBaseUrl).Returns(newBaseUrl);

            string bodyTemplate = "{{DOMAIN URL}} {{MEDIASSIST LOGO}}";
            string expected = $"{newBaseUrl} {newBaseUrl}/images/logo.png";

            // Act
            string result = (string)_resolveCommonVariablesMethod.Invoke(
                _service,
                new object[] { bodyTemplate });

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }



        [Test]
        public void ResolveCommonVariablesInSubject_WithValidSubject_ReplacesPlaceholder()
        {
            // Arrange
            string emailSubject = "Reset Password for {{DOMAIN NAME}}";
            string expected = "Reset Password for test.mediassist.com";

            // Act
            string result = (string)_resolveCommonVariablesInSubjectMethod.Invoke(
                _service,
                new object[] { emailSubject });

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ResolveCommonVariablesInSubject_WithoutPlaceholder_ReturnsUnchangedSubject()
        {
            // Arrange
            string emailSubject = "Reset Password";

            // Act
            string result = (string)_resolveCommonVariablesInSubjectMethod.Invoke(
                _service,
                new object[] { emailSubject });

            // Assert
            Assert.That(result, Is.EqualTo(emailSubject));
        }

        [Test]
        public void ResolveCommonVariablesInSubject_WithMultiplePlaceholders_ReplacesAllInstances()
        {
            // Arrange
            string emailSubject = "{{DOMAIN NAME}} - Reset Password - {{DOMAIN NAME}}";
            string expected = "test.mediassist.com - Reset Password - test.mediassist.com";

            // Act
            string result = (string)_resolveCommonVariablesInSubjectMethod.Invoke(
                _service,
                new object[] { emailSubject });

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ResolveCommonVariablesInSubject_WithNullSubject_ThrowsArgumentException()
        {
            // Arrange
            string emailSubject = null;

            // Act & Assert
            var exception = Assert.Throws<TargetInvocationException>(() =>
                _resolveCommonVariablesInSubjectMethod.Invoke(
                    _service,
                    new object[] { emailSubject }));

            Assert.That(exception.InnerException, Is.TypeOf<ArgumentException>());
            Assert.That(exception.InnerException.Message,
                Is.EqualTo("Reset password email template cannot be null or empty."));
        }

        [Test]
        public void ResolveCommonVariablesInSubject_WithEmptySubject_ThrowsArgumentException()
        {
            // Arrange
            string emailSubject = string.Empty;

            // Act & Assert
            var exception = Assert.Throws<TargetInvocationException>(() =>
                _resolveCommonVariablesInSubjectMethod.Invoke(
                    _service,
                    new object[] { emailSubject }));

            Assert.That(exception.InnerException, Is.TypeOf<ArgumentException>());
            Assert.That(exception.InnerException.Message,
                Is.EqualTo("Reset password email template cannot be null or empty."));
        }

        [Test]
        public void ResolveCommonVariablesInSubject_WithWhitespaceSubject_ThrowsArgumentException()
        {
            // Arrange
            string emailSubject = "   ";

            // Act & Assert
            var exception = Assert.Throws<TargetInvocationException>(() =>
                _resolveCommonVariablesInSubjectMethod.Invoke(
                    _service,
                    new object[] { emailSubject }));

            Assert.That(exception.InnerException, Is.TypeOf<ArgumentException>());
            Assert.That(exception.InnerException.Message,
                Is.EqualTo("Reset password email template cannot be null or empty."));
        }


        [Test]
        public void Constructor_WithValidDependencies_ShouldNotThrowException()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => new EmailManagementService(
                _mockAppSettings.Object,
                _mockMailService.Object,
                _mockUserRepository.Object,
                _mockEmailRepository.Object));
        }


        [Test]
        public async Task SendResetPasswordEmail_ValidParameters_ReturnsTrue()
        {
            // Arrange
            var firstName = "John";
            var email = "john@example.com";
            var resetCode = "ABC123";
            var masterEmailTemplate = new Master_EmailTemplate { HTMLBody = "<html>{{USER FIRST NAME}}</html>", Subject = "Reset Password" };

            _mockEmailRepository.Setup(repo => repo.Get_EmailTemplate(EmailIdentifiers.ResetRequest))
                .ReturnsAsync(masterEmailTemplate);

            _mockMailService.Setup(mail => mail.SendEmailToUserAsync(
                    It.IsAny<string>(), 
                    email,               
                    "Reset Password",    
                    It.IsAny<string>(), 
                    It.IsAny<string>(), 
                    It.IsAny<string>()  
                ))
                .ReturnsAsync(true);

            // Act
            var result = await _service.SendResetPasswordEmail(firstName, email, resetCode);

            // Assert
            Assert.That(result, Is.True);

            _mockMailService.Verify(mail => mail.SendEmailToUserAsync(
                    It.IsAny<string>(),
                    email,               
                    "Reset Password",   
                    It.IsAny<string>(),  
                    It.IsAny<string>(),  
                    It.IsAny<string>()   
                ), Times.Once);
        }

        [Test]
        public void SendResetPasswordEmail_InvalidParameters_ThrowsArgumentException()
        {
            // Arrange
            var firstName = "";
            var email = "john@example.com";
            var resetCode = "ABC123";

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => _service.SendResetPasswordEmail(firstName, email, resetCode));
        }

        #endregion

        #region SENDREQUESTDEMOEMAIL

        [Test]
        public async Task SendRequestDemoEmail_ValidParameters_LogsRequest_ReturnsTrue()
        {
            // Arrange
            var name = "Alice";
            var email = "alice@example.com";
            var phoneNumber = "1234567890";
            var requirements = "Need a demo for product X";
            var masterEmailTemplate = new Master_EmailTemplate { HTMLBody = "<html>{{USER FIRST NAME}}</html>", Subject = "Reset Password" };

            _mockEmailRepository
                .Setup(repo => repo.Get_EmailTemplate(EmailIdentifiers.RequestDemo))
                .ReturnsAsync(masterEmailTemplate);
            _mockMailService
                .Setup(mail => mail.SendEmailToSupportAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _service.SendRequestDemoEmail(name, email, phoneNumber, requirements);

            // Assert
            Assert.IsTrue(result);
            _mockUserRepository.Verify(repo => repo.LogDemoRequest(It.IsAny<DemoRequestLog>()), Times.Once);
        }

        [Test]
        public void SendRequestDemoEmail_MissingNameOrEmail_ThrowsArgumentException()
        {
            // Arrange
            string name = null; // Missing name
            string email = "alice@example.com";
            string phoneNumber = "1234567890";
            string requirements = "Need a demo for product X";

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(
                async () => await _service.SendRequestDemoEmail(name, email, phoneNumber, requirements)
            );

            Assert.AreEqual("Name and email are required for sending demo request email.", exception.Message);
        }

        #endregion

        #region CONTACTUSFORSUBSCRIPTION

        [Test]
        public async Task ContactUsForSubscription_ValidParameters_ReturnsTrue()
        {
            // Arrange
            var name = "Bob";
            var email = "bob@example.com";
            var phoneNumber = "9876543210";
            var additionalNotes = "Interested in premium plan.";
            var selectedPlan = "Premium";
            var registeredUserTemplate = new Master_EmailTemplate { HTMLBody = "<html>{{USER FIRST NAME}}</html>", Subject = "Plan Inquiry from Registered User" };
            var guestUserTemplate = new Master_EmailTemplate { HTMLBody = "<html>{{USER FIRST NAME}}</html>", Subject = "New Paid Plan Inquiry" };
            var thankYouTemplate = new Master_EmailTemplate { HTMLBody = "<html>Thank you, {{USER FIRST NAME}}!</html>", Subject = "Thank You for Your Inquiry" };

            // Mock the Get_EmailTemplate method for both registered and guest users
            _mockEmailRepository.Setup(repo => repo.Get_EmailTemplate(EmailIdentifiers.ContactUsForSubscriptionForRegisteredUserTemplate))
                .ReturnsAsync(registeredUserTemplate);
            _mockEmailRepository.Setup(repo => repo.Get_EmailTemplate(EmailIdentifiers.ContactUsForSubscriptionForGuestUserTemplate))
                .ReturnsAsync(guestUserTemplate);

            // Mock the Get_EmailTemplate method for the Thank You email template
            _mockEmailRepository.Setup(repo => repo.Get_EmailTemplate(EmailIdentifiers.ThankyouMailToUserOnPricingEnquiry))
                .ReturnsAsync(thankYouTemplate);

            // Mock the mail service to return true
            _mockMailService.Setup(mail => mail.SendEmailToSupportAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            // Mock the SendEmailToUserAsync method for sending Thank You email
            _mockMailService.Setup(mail => mail.SendEmailToUserAsync(It.IsAny<string>(), email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _service.ContactUsForSubscription(name, email, phoneNumber, additionalNotes, selectedPlan);

            // Assert
            Assert.IsTrue(result);
            _mockMailService.Verify(mail => mail.SendEmailToSupportAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockMailService.Verify(mail => mail.SendEmailToUserAsync(It.IsAny<string>(), email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ContactUsForSubscription_MissingNameEmailOrSelectedPlan_ThrowsArgumentException()
        {
            // Arrange
            string name = null; // Missing name
            string email = "bob@example.com";
            string phoneNumber = "9876543210";
            string additionalNotes = "Interested in premium plan.";
            string selectedPlan = "Premium";

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(
                async () => await _service.ContactUsForSubscription(name, email, phoneNumber, additionalNotes, selectedPlan)
            );

            Assert.AreEqual("Name, email, and selected plan are required for subscription inquiry.", exception.Message);
        }

        #endregion

        #region SENDTHANKYOUMAILFORPRICINGINQUIRY

        [Test]
        public async Task SendThankYouMailForPricingInquiry_ValidParameters_ReturnsTrue()
        {
            // Arrange
            var name = "Charlie";
            var email = "charlie@example.com";
            var emailTemplate = new Master_EmailTemplate { HTMLBody = "<html>{{USER NAME}}</html>", Subject = "Thank You for Your Inquiry!" };

            _mockEmailRepository
                .Setup(repo => repo.Get_EmailTemplate(EmailIdentifiers.ThankyouMailToUserOnPricingEnquiry))
                .ReturnsAsync(emailTemplate);

            _mockMailService.Setup(mail => mail.SendEmailToUserAsync(
                     It.IsAny<string>(), // For 'body'
                     email,               // For 'recipient'
                     "Thank You for Your Inquiry!", // Correct subject
                     It.IsAny<string>(),  // For 'attachmentPath' (optional, pass null or any value)
                     It.IsAny<string>(),  // For 'reportName' (optional, pass null or any value)
                     It.IsAny<string>()   // For 'patientName' (optional, pass null or any value)
                 ))
                 .ReturnsAsync(true);

            // Act
            var result = await _service.SendThankYouEmailForInquiry(name, email);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void SendThankYouMailForPricingInquiry_MissingNameOrEmail_ThrowsArgumentException()
        {
            // Arrange
            string name = null; // Missing name
            string email = "charlie@example.com";

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(
                async () => await _service.SendThankYouEmailForInquiry(name, email)
            );

            Assert.AreEqual("Name and email are required for sending thank you email.", exception.Message);
        }

        #endregion

        #region SENDREPORTREQUESTEMAIL

        [Test]
        public async Task SendReportRequestEmail_ValidParameters_ReturnsTrue()
        {
            // Arrange
            var file = "report.pdf";
            var email = "patient@example.com";
            var name = "John Doe";
            var hospitalName = "City Hospital";
            var hospitalAddress = "123 Main St, City";
            var reportName = "Consultation Report";
            var patientName = "Jane Smith";
            var doctorName = "Dr. Smith";
            var consultationDate = "2025-01-22";
            var emailTemplate = new Master_EmailTemplate { HTMLBody = "<html>{{USER FIRST NAME}}</html>", Subject = "Report: {{REPORT NAME}}" };

            _mockEmailRepository
                .Setup(repo => repo.Get_EmailTemplate(EmailIdentifiers.ShareReport))
                .ReturnsAsync(emailTemplate);

            _mockMailService
                .Setup(mail => mail.SendEmailToUserAsync(It.IsAny<string>(), email, It.IsAny<string>(), file, reportName, patientName))
                .ReturnsAsync(true);

            // Act
            var result = await _service.SendReportRequestEmail(file, email, name, hospitalName, hospitalAddress, reportName, patientName, doctorName, consultationDate);

            // Assert
            Assert.IsTrue(result);
            _mockMailService.Verify(mail => mail.SendEmailToUserAsync(It.IsAny<string>(), email, It.IsAny<string>(), file, reportName, patientName), Times.Once);
        }

        [Test]
        public void SendReportRequestEmail_MissingOrInvalidEmailTemplate_ThrowsException()
        {
            // Arrange
            var file = "report.pdf";
            var email = "patient@example.com";
            var name = "John Doe";
            var hospitalName = "City Hospital";
            var hospitalAddress = "123 Main St, City";
            var reportName = "Consultation Report";
            var patientName = "Jane Smith";
            var doctorName = "Dr. Smith";
            var consultationDate = "2025-01-22";

            // Simulate missing or invalid email template
            _mockEmailRepository
                .Setup(repo => repo.Get_EmailTemplate(EmailIdentifiers.ShareReport))
                .ReturnsAsync((Master_EmailTemplate)null);

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(
                async () => await _service.SendReportRequestEmail(file, email, name, hospitalName, hospitalAddress, reportName, patientName, doctorName, consultationDate)
            );

            Assert.IsTrue(exception.Message.Contains("An error occurred while processing your request"));
            Assert.IsTrue(exception.InnerException is InvalidOperationException);
            Assert.AreEqual("Thank you email template is missing or invalid.", exception.InnerException.Message);
        }

        #endregion
    }
}
