using MediAssist.Infrastructure.HttpProvider.Services;
using MediAssist.Infrastructure.HttpProvider.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace MediAssist.Infrastructure.HttpProvider
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastuctureHttpProvider(this IServiceCollection services)
        {
            services.AddScoped<IMailService, Mailservice>();
            services.AddHttpClient<IFHIRHttpProvider, FHIRHttpProvider>();
            services.AddScoped<IFHIRHttpProvider, FHIRHttpProvider>();
            services.AddSingleton<IAzureKeyVaultService, AzureKeyVaultService>();
            return services;
        }
    }
}
