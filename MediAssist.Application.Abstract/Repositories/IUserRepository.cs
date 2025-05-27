using MediAssist.Application.Abstract.Entities;
using MediAssist.DbContext;
using Microsoft.AspNetCore.Identity;

namespace MediAssist.Application.Abstract.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<DoctorProfile?> GetUserDetailsbyIdAsync(string userId);
        Task<UserTitle> GetUserTitlebyIdAsync(int? titleId);
        Task<bool> VerifyPasswordAsync(ApplicationUser user, string password);
        Task<IdentityResult> CreateUserAsync(string firstName, string middleName, string lastName, ISignUpUserDetails signUpUserDetails);
        Task<ApplicationUser?> GetUserByUserIdAsync(string userId);
        Task<Feedback> AddFeedbackAsync(Feedback feedback);
        Task<FeedbackCategoryMapping> AddCategoryFeedbackAsync(FeedbackCategoryMapping feedbackCategoryMapping);
        Task<List<Feedback>> GetUserFeedBacksByIdAsync(string userId);
        Task<Clinic?> GetUserClinicDetailsByIdAsync(int? clinicId);
        Task<bool> CheckWhetherUserHistoryIsExist(string email);
        void LogDemoRequest(DemoRequestLog requestLog);

        Task<int?> AddPatientAsync(string firstName, string lastName, IPatientDetails patientDetails);

        Task<Patient?> GetPatientByIdAsync(int? patientId);

        Task<bool> UpdatePatientAsync(Patient patient);

        Task<bool> AddEncounterAsync(int patientId, string userId);

        Task<string?> GetUserIdByClinicIdAsync(int clinicId);

    }
}
