using CSharpFunctionalExtensions;
using Framework.Core.Enums;
using System.Linq;
using System.Threading.Tasks;
using VDVI.Services.Interfaces.AFAS;
using Rebus.Bus;
using Microsoft.Extensions.Configuration;
using System;
using VDVI.Services.Rebus.Models;
using VDVI.Services.Interfaces.Scheduler.AFAS;

namespace VDVI.Services.AFAS
{
    public class AfasTaskSchedulerService : IAfasTaskSchedulerService
    {
        private readonly IAfasSchedulerSetupService _afasschedulerSetupService;
        private readonly IBus _eventBus;

        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        public IConfiguration _config; 

        public AfasTaskSchedulerService(
              IAfasSchedulerSetupService afasschedulerSetupService,
                IBus eventBus
            )
        {
            configurationBuilder.AddJsonFile("AppSettings.json");
            _config = configurationBuilder.Build();
            _afasschedulerSetupService = afasschedulerSetupService;
            _eventBus = eventBus;
        }


        public async Task SummaryScheduler()
        { 

             var afasschedulers = await _afasschedulerSetupService.FindByAllScheduleAsync();
            var logDayLimits = Convert.ToInt32(_config.GetSection("SchedulerLog").GetSection("AFASSchedulerLogLimitDays").Value);

            var new1 = afasschedulers.ToList();

            for (int i = 0; i < new1.Count(); i++)
            {
                var afasscheduler = new1[i];

                if (afasscheduler.SchedulerStatus == SchedulerStatus.Processing.ToString())
                    continue;

                if (
                        afasscheduler.NextExecutionDateTime != null
                        && afasscheduler.NextExecutionDateTime <= DateTime.UtcNow
                    )
                {
                    //Update SchedulerSetUp Status;
                    afasscheduler.SchedulerStatus= SchedulerStatus.Processing.ToString();
                    await _afasschedulerSetupService.UpdateAsync(afasscheduler);
                     

                    AfasSchedulerEvent afasSchedulerEvent = new AfasSchedulerEvent
                    {
                        Scheduler = afasscheduler,
                        BusinessStartDate = afasscheduler.BusinessStartDate
                    };

                    // Send notification to apma handlers
                    await _eventBus.SendLocal(afasSchedulerEvent);
                }

                //
            }
        }

        public async Task ResetStatusAsync()
        {
            await _afasschedulerSetupService.ResetStatusAsync();
        } 
    }
}
