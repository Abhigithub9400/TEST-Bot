using MediAssist.Application.Abstract.Repositories;
using MediAssist.Application.Abstract.Services;
using MediAssist.Application.Services;
using MediAssist.Application.Services.FHIRServices;
using MediAssist.Configurations;
using MediAssist.DataAccess.Repository;
using MediAssist.Infrastructure.Abstract.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace MediAssist.Dependency
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IProfileManagementService, ProfileManagementService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IEmailManagementService, EmailManagementService>();
            services.AddScoped<IUserSessionService, UserSessionService>();
            services.AddTransient<IPlanExpiryHandlerService, PlanExpiryHandlerService>();
            services.AddScoped<IPatientService, PatientService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<IFHIRMappingRepository, FHIRMappingRepository>();
            services.AddScoped<IFHIRServiceExecutor, FHIRServiceExecutor>();
            services.AddScoped<IFHIRServiceFactory, FHIRServiceFactory>();
            services.AddScoped<IFHIRService, PatientFHIRServices>();
            services.AddScoped<IFHIRService, OrganizationFHIRServices>();


            services.AddScoped<IAppSettings, Appsettings>();
            
               

            return services;
        }
    }
}
