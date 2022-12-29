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
using VDVI.Client.Configurations;
using VDVI.Client.IoC;
using VDVI.Services.Configurations;
using VDVI.Services.Interfaces.AFAS;
using VDVI.Services.Interfaces.APMA;
using StartupBase = Framework.Core.Base.Startup.StartupBase;
using Rebus.ServiceProvider;
using VDVI.Services.Interfaces.Scheduler.AFAS;

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


            // Add Afas Configuration
            services.AddAfasConfig(Configuration);

            // Add Scheduler Log config
            services.AddSchedulerLogConfig(Configuration, "SchedulerLog");

            //Hangfire
            services.AddHangfire(config =>
                       config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                       .UseSimpleAssemblyNameTypeSerializer()
                       .UseDefaultTypeSerializer()
                       .UseSqlServerStorage(Configuration.GetConnectionString("HangfireDb")
                       ));


            services.AddHangfireServer();
            services.AddMvc();


            // Rebus
            services.AddRebus(Configuration);
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider,
            IConfiguration configuration,
            IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider,
            IUnityContainer container,
            System.Collections.Generic.IEnumerable<Hangfire.Dashboard.IDashboardAuthorizationFilter> hangfireAuthorizationFilters

        )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Startup Query Execution
            app.UseStartupQuery();


            app.UseDefaultFiles()
               .UseStaticFiles();

            // Use the CORS policy
            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseRouting();

             

            app.UseSerilogRequestLogging();

             

            // Enable compression
            app.UseResponseCompression();


            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                DashboardTitle = "Scheduled Jobs",
                Authorization = hangfireAuthorizationFilters
            });

            // to remove existing hangfire
            recurringJobManager.RemoveIfExists("ApmaJob");
            recurringJobManager.RemoveIfExists("AfasJob");



            // Use Rebus
            app.UseRebus(r => r.AddSubscriptions());


            var apmaservice = container.Resolve<IApmaTaskSchedulerService>();
            var afasservice = container.Resolve<IAfasTaskSchedulerService>();

            recurringJobManager.AddOrUpdate(
              "ApmaJob",
              () => apmaservice.SummaryScheduler(),
              configuration["HangfireJobSchedulerTime:ApmaJob"], TimeZoneInfo.Utc
              );

            recurringJobManager.AddOrUpdate(
             "AfasJob",
             () => afasservice.SummaryScheduler(),
             configuration["HangfireJobSchedulerTime:AfasJob"], TimeZoneInfo.Utc
             );

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
