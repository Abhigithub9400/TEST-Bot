using MediAssist.Application.Abstract.Entities;
using MediAssist.DbContext;

namespace MediAssist.Application.Abstract.Services
{
    public interface ISettingsService
    {
        Task<IServiceResponse<Clinic>> UpdateClinic(IClinicDetails clinicDetails);
        Task<IServiceResponse<Clinic>> GetClinicByUserIdAsync(string userId);
        Task<IServiceResponse<DoctorProfile>> UpdateDoctorProfile(IDoctorProfileSettings doctorDetailsSettings);
        Task<IServiceResponse<IReportData>> GetClinicAndDoctorDetailsAsync(ApplicationUser user);
        Task<bool> CheckWhetherSettingsUpdated(ApplicationUser user);
    }
}
