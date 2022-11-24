using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VDVI.Services.Interfaces.AFAS;
using VDVI.Services.Interfaces.APMA;

namespace VDVI.Client.Configurations
{
    public static class StartupQueryConfig
    {
        public static IApplicationBuilder UseStartupQuery(this IApplicationBuilder app)
        {
            // Reset APMA Scheduler Status when task is processing
            var apmaTaskSchedulerService = app.ApplicationServices.GetRequiredService<IApmaTaskSchedulerService>();

            if (apmaTaskSchedulerService != null)
            {
                apmaTaskSchedulerService.ResetStatusAsync().GetAwaiter().GetResult();
            }

            // Reset AFAS Scheduler Status when task is processing
            var afasSchedulerSetupService = app.ApplicationServices.GetRequiredService<IAfasSchedulerSetupService>();

            if (afasSchedulerSetupService != null)
            {
                afasSchedulerSetupService.ResetStatusAsync().GetAwaiter().GetResult();
            }

            return app;
        }
    }
}
