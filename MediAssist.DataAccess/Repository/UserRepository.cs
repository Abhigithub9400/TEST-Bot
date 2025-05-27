using MediAssist.Application.Abstract.Entities;
using MediAssist.Application.Abstract.Repositories;
using MediAssist.DbContext;
using MediAssist.Infrastructure.Extensions.Common;
using MediAssist.Infrastructure.HttpProvider.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MediAssist.DataAccess.Repository
{

    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MediAssistDbContext _context;
        private readonly IAzureKeyVaultService _keyVault;

        public UserRepository(UserManager<ApplicationUser> userManager,
                              MediAssistDbContext context,
                              IAzureKeyVaultService keyVault)
        {
            _userManager = userManager;
            _context = context;
            _keyVault = keyVault;
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<DoctorProfile?> GetUserDetailsbyIdAsync(string userId)
        {
            return await _context.DoctorProfiles.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<List<Feedback>> GetUserFeedBacksByIdAsync(string userId)
        {
            return await _context.Feedbacks.Where(f => f.UserId == userId).ToListAsync();
        }

        public async Task<Clinic?> GetUserClinicDetailsByIdAsync(int? clinicId)
        {
            return await _context.Clinics.FirstOrDefaultAsync(dp => dp.Id == clinicId);
        }

        public async Task<bool> CheckWhetherUserHistoryIsExist(string email)
        {
            var result = await _context.UsersHistories.FirstOrDefaultAsync(dp => dp.EmailAddress == email);
            if (result == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<UserTitle> GetUserTitlebyIdAsync(int? titleId)
        {
            return await _context.UserTitles.FirstOrDefaultAsync(x => x.Id == titleId);
        }

        public async Task<bool> VerifyPasswordAsync(ApplicationUser user, string password)
            {
                
                return await _userManager.CheckPasswordAsync(user, password);
            }

        public async Task<IdentityResult> CreateUserAsync(string firstName, string middleName, string lastName, ISignUpUserDetails signUpUserDetails)
        {
            var userData = await _userManager.FindByEmailAsync(signUpUserDetails.Email);

            if (userData == null)
            {
                var user = new ApplicationUser
                {
                    UserName = signUpUserDetails.Email,
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName,
                    FullName = signUpUserDetails.FullName,
                    Email = signUpUserDetails.Email,
                    PasswordHash = signUpUserDetails.Password,
                    TermsAndPrivacy = signUpUserDetails.TermsAndPrivacy,
                    LicenseAgreement = signUpUserDetails.LicenseAgreement
                };


                var result = await _userManager.CreateAsync(user, signUpUserDetails.Password);

                if (result.Succeeded)
                {
                    var userDetails = new DoctorProfile
                    {
                        UserId = user.Id,
                        Title = signUpUserDetails.Title,
                        Gender = signUpUserDetails.Gender,
                        DOB = signUpUserDetails.DateOfBirth,
                        MedicalCredentials = signUpUserDetails.MedicalCredentials,
                        Specialization = signUpUserDetails.Specialization
                    };

                    _context.DoctorProfiles.Add(userDetails);

                    await _context.SaveChangesAsync();
                }

                return result;
            }
            else
            {
                throw new InvalidOperationException("There was an error submitting the form. Please try again later.");
            }
        }
        public async Task<ApplicationUser?> GetUserByUserIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }


        public async Task<Feedback> AddFeedbackAsync(Feedback feedback)
        {

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            return feedback;
             
        }
        public async Task<FeedbackCategoryMapping> AddCategoryFeedbackAsync(FeedbackCategoryMapping feedbackCategoryMapping)
        {

            _context.feedbackCategoryMappings.Add(feedbackCategoryMapping);
            await _context.SaveChangesAsync();
            return feedbackCategoryMapping;
             
        }

        public void LogDemoRequest(DemoRequestLog requestLog)
        {
            _context.DemoRequestLogs.Add(requestLog);
            _context.SaveChanges();
        }

        public async Task<int?> AddPatientAsync( string firstName,string lastName,IPatientDetails  patientDetails)
        {
                string key = await _keyVault.GetSecretAsync("Encryption-Key");
                string iv = await _keyVault.GetSecretAsync("Encryption-IV");

               // Encrypt sensitive fields before storing them
                var encryptedFirstName = EncryptionHelper.Encrypt(firstName, key, iv);
                var encryptedLastName = EncryptionHelper.Encrypt(lastName, key, iv);
                var encryptedMrn = EncryptionHelper.Encrypt(patientDetails.MrnNumber.ToString(), key, iv);
                var encryptedAge = EncryptionHelper.Encrypt(patientDetails.Age.ToString(), key, iv);
                var encryptedDOB = EncryptionHelper.Encrypt(patientDetails.DOB, key, iv);
                var encryptedGender = EncryptionHelper.Encrypt(patientDetails.Gender, key, iv);

                var newPatient = new Patient()
                {
                    FirstName = encryptedFirstName,
                    LastName = encryptedLastName,
                    MrnNumber = encryptedMrn,
                    Age = encryptedAge,
                    DOB = encryptedDOB,
                    Gender = encryptedGender,
                    IsUpdatedtoFHIR = false,
                    ClinicId = patientDetails.ClinicId,
                    CreatedBy = patientDetails.UserId,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = patientDetails.UserId,
                    ModifiedDate = DateTime.Now,
                    PatientConsent = patientDetails.PatientConsent
                };
                 _context.Patients.Add(newPatient);
               
                var result = await _context.SaveChangesAsync();
                return result > 0 ? newPatient.PatientId : null;

        }

        public async Task<Patient?> GetPatientByIdAsync(int? patientId)
        {
            if(patientId == null)
            {
                return null;
            }

            return await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == patientId);
        }

        public async Task<bool> UpdatePatientAsync(Patient patient)
        {
            if (patient == null)
            {
                return false;
            }

            var existingPatient = await _context.Patients.FindAsync(patient.PatientId);

            if (existingPatient == null)
            {
                return false;
            }

            string key = await _keyVault.GetSecretAsync("Encryption-Key");
            string iv = await _keyVault.GetSecretAsync("Encryption-IV");

            existingPatient.FirstName = EncryptionHelper.Encrypt(patient.FirstName, key, iv);
            existingPatient.LastName = EncryptionHelper.Encrypt(patient.LastName, key, iv);
            existingPatient.MrnNumber = EncryptionHelper.Encrypt(patient.MrnNumber.ToString(), key, iv);
            existingPatient.Age = EncryptionHelper.Encrypt(patient.Age.ToString(), key, iv);
            existingPatient.Gender = EncryptionHelper.Encrypt(patient.Gender, key, iv);
            existingPatient.IsUpdatedtoFHIR = false;
            existingPatient.ModifiedBy = patient.ModifiedBy;
            existingPatient.ModifiedDate = patient.ModifiedDate;

            _context.Patients.Update(existingPatient);

            var result = await _context.SaveChangesAsync();

            if (result > 0 )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> AddEncounterAsync(int patientId, string userId)
        {
            var doctorPatientMapping = new DoctorPatientMapping
            {
                Patient_id = patientId,
                Practitioner_id = userId,
                IsUpdatedtoFHIR = false,
                CreatedBy = userId,
                CreatedDate = DateTime.Now,
                ModifiedBy = userId,
                ModifiedDate = DateTime.Now
            };

            _context.DoctorPatientMapping.Add(doctorPatientMapping);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<string?> GetUserIdByClinicIdAsync(int clinicId)
        {
            return await _context.DoctorProfiles
                .Where(d => d.ClinicId == clinicId)
                .Select(d => d.UserId)
                .FirstOrDefaultAsync();
        }
    }
}


