using MediAssist.Application.Abstract.Entities;
using MediAssist.Application.Abstract.Repositories;
using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Entities;
using MediAssist.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MediAssist.Application.Services
{
    public class SettingsService : ISettingsService
    {
        #region PRIVATE FIELDS
        private readonly MediAssistDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private Clinic updatedClinic;
        private readonly ILogger<SettingsService> _logger;

        #endregion

        #region CONSTRUCTOR
        public SettingsService(MediAssistDbContext context, UserManager<ApplicationUser> userManager, IUserRepository userRepository,ILogger<SettingsService> logger)
        {
            _context = context;
            _userManager = userManager;
            _userRepository = userRepository;
            _logger = logger;
        }
        #endregion

        #region PUBLIC METHODS
        public async Task<IServiceResponse<Clinic>> UpdateClinic(IClinicDetails clinicDetails)
        {
            try
            {
                var doctorProfile = await _context.DoctorProfiles
                                                .Include(d => d.Clinic)
                                                .FirstOrDefaultAsync(d => d.UserId == clinicDetails.UserId);

                if (doctorProfile is null)
                {
                    return new ServiceResponse<Clinic>
                    {
                        Success = false,
                        Message = "Doctor profile not found",
                        Data = null
                    };
                }

                var logo = ConvertBase64ToByteArray(clinicDetails.Logo);

                if (doctorProfile.ClinicId == null)
                {
                    updatedClinic = await AddClinicDetails(clinicDetails, doctorProfile, logo);
                }
                else
                {
                    updatedClinic = await UpdateClinicDetails(clinicDetails, doctorProfile, logo);
                }

                return new ServiceResponse<Clinic>
                {
                    Success = true,
                    Message = "Clinic details saved successfully",
                    Data = updatedClinic
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                return new ServiceResponse<Clinic>
                {
                    Success = false,
                    Message = $"Error on updating clinic details: {ex.Message}",
                    Data = null
                };
            }
        }

        

        public async Task<IServiceResponse<Clinic>> GetClinicByUserIdAsync(string userId)
        {
            try
            {
                var doctorProfile = await _context.DoctorProfiles
                .Include(d => d.Clinic)
                .FirstOrDefaultAsync(d => d.UserId == userId);

                if (doctorProfile.Clinic is null)
                {
                    return new ServiceResponse<Clinic>
                    {
                        Success = false,
                        Message = "Clinic details not found",
                        Data = null
                    };
                }

                return new ServiceResponse<Clinic>
                {
                    Success = true,
                    Message = "Clinic details fetched successfully",
                    Data = doctorProfile.Clinic
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                return new ServiceResponse<Clinic>
                {
                    Success = false,
                    Message = $"Error getting clinic details: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<IServiceResponse<IReportData>> GetClinicAndDoctorDetailsAsync(ApplicationUser user)
        {
            try
            {
                var doctorProfile = await _context.DoctorProfiles
                .Include(d => d.Clinic)
                .FirstOrDefaultAsync(d => d.UserId == user.Id);

                if (doctorProfile is null)
                {
                    return new ServiceResponse<IReportData>
                    {
                        Success = false,
                        Message = "Doctor's profile not found"
                    };
                }

                var userTitle = await _userRepository.GetUserTitlebyIdAsync(doctorProfile.Title);

                string signBase64 = null;
                if (doctorProfile?.Signature != null && doctorProfile?.Signature?.Length > 0)
                {
                    signBase64 = Convert.ToBase64String(doctorProfile.Signature);
                }

                string logoBase64 = null;
                if (doctorProfile?.Clinic?.Logo != null && doctorProfile?.Clinic?.Logo.Length > 0)
                {
                    logoBase64 = Convert.ToBase64String(doctorProfile.Clinic.Logo);
                }

                var reportData = new ReportData()
                {
                    DoctorName = user.FullName,
                    DoctorSpecialization = doctorProfile.Specialization,
                    DoctorTitle = userTitle.Abbreviations,
                    DoctorSignature = signBase64,
                    HospitalName = doctorProfile.Clinic.Name,
                    HospitalAddress = doctorProfile.Clinic.Address,
                    HospitalLogo = logoBase64,
                    ClinicId = doctorProfile.Clinic.Id
                };

                return new ServiceResponse<IReportData>
                {
                    Success = true,
                    Message = "Report details fetched successfully",
                    Data = reportData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                return new ServiceResponse<IReportData>
                {
                    Success = false,
                    Message = $"Error getting clinic details: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<IServiceResponse<DoctorProfile>> UpdateDoctorProfile(IDoctorProfileSettings doctorDetailsSettings)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(doctorDetailsSettings.UserId);

                if (user is null)
                {
                    return new ServiceResponse<DoctorProfile>
                    {
                        Success = false,
                        Message = "User not found",
                        Data = null
                    };
                }
                
                var doctorProfile = await _context.DoctorProfiles.FirstOrDefaultAsync(d => d.UserId == doctorDetailsSettings.UserId);

                if (doctorProfile is null)
                {
                    return new ServiceResponse<DoctorProfile>
                    {
                        Success = false,
                        Message = "Doctor's profile not found",
                        Data = null
                    };
                }
                
                var signature = ConvertBase64ToByteArray(doctorDetailsSettings.Signature);

                await MapDoctorProfile(doctorDetailsSettings, doctorProfile, signature);
                _context.DoctorProfiles.Update(doctorProfile);
                
                await _context.SaveChangesAsync();
                
                return new ServiceResponse<DoctorProfile>
                {
                    Success = true,
                    Message = "Doctor's information updated succesfully.",
                    Data = doctorProfile
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                return new ServiceResponse<DoctorProfile>
                {
                    Success = false,
                    Message = "Failed to save settings. Please try again.",
                    Data = null
                };
            }
        }

        public async Task<bool> CheckWhetherSettingsUpdated(ApplicationUser user)
        {
            try
            {

                var userDetails = await _userRepository.GetUserDetailsbyIdAsync(user.Id);

                var isSettingsUpdated = true;

                if (userDetails?.Signature == null || userDetails?.ClinicId == null)
                {
                    isSettingsUpdated = false;
                }

                return isSettingsUpdated;
            }
            catch (Exception ex) {
                _logger.LogError("An error occurred: {Message}. See exception details:", ex.Message);
                throw;
            }
        }
        #endregion

        #region PRIVATE METHODS
        private async Task MapDoctorProfile(IDoctorProfileSettings doctorDetailsSettings, DoctorProfile doctorProfile, byte[]? signature)
        {
            doctorProfile.Specialization = doctorDetailsSettings.Specialization;
            doctorProfile.MedicalCredentials = doctorDetailsSettings.MedicalCredentials;
            doctorProfile.Signature = signature;
        }

        private byte[]? ConvertBase64ToByteArray(string image)
        {
            if (!string.IsNullOrEmpty(image))
            {
                if (image.StartsWith("data:image/png;base64,"))
                {
                    image = image.Substring("data:image/png;base64,".Length);
                }
                else if (image.StartsWith("data:image/jpeg;base64,"))
                {
                    image = image.Substring("data:image/jpeg;base64,".Length);
                }

                byte[] signature = Convert.FromBase64String(image);
                return signature;
            }
            return null;
        }

        private async Task<Clinic> UpdateClinicDetails(IClinicDetails clinicDetails, DoctorProfile doctorProfile, byte[]? logo)
        {
            var existingClinic = doctorProfile.Clinic;

            existingClinic.Name = clinicDetails.ClinicName;
            existingClinic.Address = clinicDetails.ClinicAddress;
            existingClinic.Email = clinicDetails.Email;
            existingClinic.PhoneNumber = clinicDetails.PhoneNumber;
            existingClinic.CountryCode = clinicDetails.CountryCode;
            existingClinic.Website = clinicDetails.Website;
            existingClinic.Logo = logo;

            _context.Clinics.Update(existingClinic);
            await _context.SaveChangesAsync();

            return existingClinic;
        }

        private async Task<Clinic> AddClinicDetails(IClinicDetails clinicDetails, DoctorProfile doctorProfile, byte[]? logo)
        {
            var clinic = new Clinic()
            {
                Name = clinicDetails.ClinicName,
                Address = clinicDetails.ClinicAddress,
                Email = clinicDetails.Email,
                PhoneNumber = clinicDetails.PhoneNumber,
                CountryCode = clinicDetails.CountryCode,
                Website = clinicDetails.Website,
                Logo = logo
            };

            _context.Clinics.Add(clinic);

            await _context.SaveChangesAsync();

            doctorProfile.ClinicId = clinic.Id;
            _context.DoctorProfiles.Update(doctorProfile);

            await _context.SaveChangesAsync();

            return clinic;
        }

        #endregion
    }
}
