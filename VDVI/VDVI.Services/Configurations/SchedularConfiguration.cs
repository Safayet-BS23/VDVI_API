using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VDVI.Repository.Models;

namespace VDVI.Services.Configurations
{
    public static class SchedularConfiguration
    {
        public static IServiceCollection AddSchedulerLogConfig(this IServiceCollection services, IConfiguration Configuration, string sectionName)
        {
            services.Configure<SchedulerLog>(Configuration.GetSection(sectionName));
            return services;
        }
    }
}
