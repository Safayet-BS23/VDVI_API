using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Enums;
using Microsoft.Extensions.Configuration;
using Rebus.Bus;
using System;
using System.Linq;
using System.Threading.Tasks;
using VDVI.DB.Dtos;
using VDVI.Services.Interfaces.APMA;
using VDVI.Services.MediatR.Models;

namespace VDVI.Services.APMA
{
    public class ApmaTaskSchedulerService : IApmaTaskSchedulerService
    {
        private readonly ISchedulerSetupService _schedulerSetupService;
        private readonly IBus _eventBus;

        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        public IConfiguration _config;

        private DateTime _startDate = new DateTime();
        private DateTime _endDate = new DateTime();

        SchedulerSetupDto dtos = new SchedulerSetupDto();
        public ApmaTaskSchedulerService
            (
                ISchedulerSetupService schedulerSetupService,
                IBus eventBus
            )
        {
            configurationBuilder.AddJsonFile("AppSettings.json");
            _config = configurationBuilder.Build();
            _schedulerSetupService = schedulerSetupService;
            _eventBus = eventBus;
        }

        public async Task SummaryScheduler()
        {
            bool flag = false;
            Result<PrometheusResponse> response;
            DateTime currentDateTime = DateTime.UtcNow;
            var logDayLimits = Convert.ToInt32(_config.GetSection("SchedulerLog").GetSection("APMASchedulerLogLimitDays").Value);

            var schedulers = await _schedulerSetupService.FindByAllScheduleAsync();
            var new1 = schedulers.ToList();

            for (int i = 0; i < new1.Count(); i++)
            {
                var scheduler = new1[i];


                if (scheduler.SchedulerStatus == SchedulerStatus.Processing.ToString())
                    continue;

                if (
                     scheduler.NextExecutionDateTime <= currentDateTime
                     &&
                     (scheduler.LastBusinessDate == null // for Initial Load Data
                         ||
                         (scheduler.isFuture == false && scheduler.LastBusinessDate.Value.Date < currentDateTime.Date) // for History Condition
                         ||
                         (scheduler.isFuture == true && scheduler.LastBusinessDate.Value.Date <= currentDateTime.Date) // for Future Condition
                     )
                  )
                {
                    //History
                    if (scheduler.isFuture == false
                        && scheduler.LastBusinessDate == null)
                    {
                        _startDate = (DateTime)scheduler.BusinessStartDate;
                        _endDate = _startDate.AddDays(scheduler.DaysLimit);
                    }
                    else if (scheduler.isFuture == false
                        && scheduler.LastBusinessDate != null)
                    {
                        _startDate = ((DateTime)scheduler.LastBusinessDate);
                        _endDate = _startDate.AddDays(scheduler.DaysLimit);
                    }

                    // for future Method
                    else if (scheduler.isFuture && scheduler.LastBusinessDate == null)
                        _startDate = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 0, 0, 0);


                    if (_endDate >= currentDateTime) _endDate = currentDateTime.AddDays(-1); // if endDate cross the CurrentDate; then endDate would be change 

                    if (_endDate.Date < _startDate.Date) _endDate = _startDate;

                    //Update SchedulerSetUp Status;
                    scheduler.SchedulerStatus = SchedulerStatus.Processing.ToString();
                    await _schedulerSetupService.UpdateAsync(scheduler);

                    ApmaSchedulerEvent schedulerEvent = new ApmaSchedulerEvent
                    {
                        Scheduler = scheduler,
                        CurrentDate = currentDateTime,
                        DaysLimit = scheduler.DaysLimit,
                        EndDate = _endDate,
                        StartDate = _startDate,
                        LogDayLimits = logDayLimits
                    };

                    // Send notification to apma handlers
                    await _eventBus.SendLocal(schedulerEvent);

                }

            }
        }

        public async Task ResetStatusAsync()
        {
           await _schedulerSetupService.ResetStatusAsync();
        }
    }
}
