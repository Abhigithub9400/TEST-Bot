namespace MediAssist.Application.Abstract.Services
{
    public interface IEmailManagementService
    {
        Task<bool> SendResetPasswordEmail(string firstName, string email, string resetCode);

        Task<bool> SendRequestDemoEmail(string name, string email, string phoneNumber, string requirements);

        Task<bool> ContactUsForSubscription(string name, string email, string phoneNumber, string additionalNotes, string selectedPlan);

        Task<bool> SendThankYouEmailForInquiry(string name, string email);

        Task<bool> SendReportRequestEmail(string file, string email, string name, string hospitalName, string hospitalAddress, string reportName, string patientName, string doctorName, string consultationDate);

        Task<bool> SendGenericEnquiryEmail(string name, string email, string phoneNumber, string requirements);
    }
}
