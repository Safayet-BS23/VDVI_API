using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using Unity;
using VDVI.Client.IoC;
using VDVI.Services;
using VDVI.Services.Interfaces.APMA; 
using StartupBase = Framework.Core.Base.Startup.StartupBase;

namespace VDVI
{
    public class Startup : StartupBase
    {
        private static string ApiTitle => "VDVI Application";
        public Startup(IConfiguration configuration) : base(configuration, new UnityDependencyProvider(), ApiTitle) { }

        // This method gets called by the runtime. Use this method to add services to the container.
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);


            //Hangfire
            services.AddHangfire(config =>
                       config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                       .UseSimpleAssemblyNameTypeSerializer()
                       .UseDefaultTypeSerializer()
                       .UseSqlServerStorage(Configuration.GetConnectionString("ApmaDb")
                       ));

            services.AddHangfireServer();
            services.AddMvc();
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider,
            IConfiguration configuration,
            IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider,
            IUnityContainer container
        )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseDefaultFiles()
               .UseStaticFiles();

            // Use the CORS policy
            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthentication();

            //app.UseAuthorization();

            app.UseSerilogRequestLogging();

            // global error handler
            //app.UseMiddleware<ApplicationAccessMiddleware>();

            //app.UseMiddleware<ErrorHandlerMiddleware>();

            // Enable compression
            app.UseResponseCompression();



            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                DashboardTitle = "Scheduled Jobs"
            });


            var service = container.Resolve<IApmaTaskSchedulerService>();

           recurringJobManager.AddOrUpdate(
             "ApmaJob",
             () => service.SummaryScheduler(),
             configuration["HangfireJobSchedulerTime:ApmaJob"], TimeZoneInfo.Utc
             ); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
