﻿using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using VDVI.DB.Dtos;
using VDVI.Services.Interfaces;
using VDVI.Services.Interfaces.APMA; 

namespace VDVI.Services.APMA
{
    public class ApmaTaskSchedulerService : IApmaTaskSchedulerService
    {
        private readonly IHcsReportManagementSummaryService _reportSummary;
        private readonly IHcsBIReservationDashboardHistoryService _hcsBIReservationDashboardHistoryService;
        private readonly IHcsBIReservationDashboardFutureService _hcsBIReservationDashboardFutureService;
        private readonly IHcsBIRatePlanStatisticsHistoryService _hcsBIRatePlanStatisticsHistoryService;
        private readonly IHcsBIRatePlanStatisticsFutureService _hcsBIRatePlanStatisticsFutureService;
        private readonly IHcsBISourceStatisticsHistoryService _hcsBISourceStatisticsHistoryService;

        private readonly IHcsBISourceStatisticsFutureService _hcsBISourceStatisticsFutureService;
        private readonly IHcsGetDailyHistoryService _hcsGetDailyHistoryService;
        private readonly IHcsGetDailyFutureService _hcsGetDailyFutureService;
        private readonly ISchedulerSetupService _schedulerSetupService;
        public readonly ISchedulerLogService _schedulerLogService;

        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        public IConfiguration _config;

        private DateTime _startDate = new DateTime();
        private DateTime _endDate = new DateTime();

        SchedulerSetupDto dtos = new SchedulerSetupDto();
        public ApmaTaskSchedulerService(
            IHcsReportManagementSummaryService reportSummary,
            IHcsBIReservationDashboardHistoryService hcsBIReservationDashboardHistoryService,
            IHcsBIReservationDashboardFutureService hcsBIReservationDashboardFutureService,
            IHcsBIRatePlanStatisticsHistoryService hcsBIRatePlanStatisticsHistoryService,
            IHcsBIRatePlanStatisticsFutureService hcsBIRatePlanStatisticsFutureService,
            IHcsBISourceStatisticsHistoryService hcsBISourceStatisticsHistoryService
            , IHcsBISourceStatisticsFutureService hcsBISourceStatisticsFutureService
            , IHcsGetDailyHistoryService hcsGetDailyHistoryService
            , IHcsGetDailyFutureService hcsGetDailyFutureService
           , ISchedulerSetupService schedulerSetupService
           , ISchedulerLogService schedulerLogService
         

            )
        {
            _reportSummary = reportSummary;
            _hcsBIReservationDashboardHistoryService = hcsBIReservationDashboardHistoryService;
            _hcsBIReservationDashboardFutureService = hcsBIReservationDashboardFutureService;
            _hcsBIRatePlanStatisticsHistoryService = hcsBIRatePlanStatisticsHistoryService;
            _hcsBIRatePlanStatisticsFutureService = hcsBIRatePlanStatisticsFutureService;
            _hcsBISourceStatisticsHistoryService = hcsBISourceStatisticsHistoryService;
            _hcsBISourceStatisticsFutureService = hcsBISourceStatisticsFutureService;
            _hcsGetDailyHistoryService = hcsGetDailyHistoryService;
            _hcsGetDailyFutureService = hcsGetDailyFutureService;
            _schedulerSetupService = schedulerSetupService;
            _schedulerLogService = schedulerLogService;

            configurationBuilder.AddJsonFile("AppSettings.json");
            _config = configurationBuilder.Build();
        }
        public async Task SummaryScheduler()
        {
            bool flag = false;
            Result<PrometheusResponse> response;
            DateTime currentDateTime = DateTime.UtcNow;
            var logDayLimits =Convert.ToInt32(_config.GetSection("SchedulerLog").GetSection("APMASchedulerLogLimitDays").Value);

            var schedulers = await _schedulerSetupService.FindByAllScheduleAsync();
            var new1 = schedulers.ToList();

            for (int i = 0; i < new1.Count(); i++)
            {
                var scheduler = new1[i];

                // LastBusinessDate marked to NextExecutionDate 
                if (scheduler.LastBusinessDate != null) scheduler.LastBusinessDate = ((DateTime)scheduler.LastBusinessDate).AddDays(1);

                if(
                     (scheduler.NextExecutionDateTime == null || scheduler.NextExecutionDateTime <= currentDateTime)
                     &&
                     ( 
                         (scheduler.isFuture ==false && scheduler.LastBusinessDate.Value.Date < currentDateTime.Date) // for History Condition
                         ||                   
                         (scheduler.isFuture == true && (scheduler.LastBusinessDate == null ||  scheduler.LastBusinessDate.Value.Date <= currentDateTime.Date)) // for Future Condition
                     )
                  )
                {
                    //History
                    if (scheduler.isFuture == false
                        && scheduler.NextExecutionDateTime == null)
                    {
                        _startDate = (DateTime)scheduler.BusinessStartDate;
                        _endDate = _startDate.AddDays(scheduler.DaysLimit);
                    }
                    else if (scheduler.isFuture == false
                        && scheduler.NextExecutionDateTime != null)
                    {
                        _startDate = ((DateTime)scheduler.LastBusinessDate);
                        _endDate = _startDate.AddDays(scheduler.DaysLimit);
                    }

                    // for future Method
                    else if (scheduler.isFuture && scheduler.NextExecutionDateTime == null)
                        _startDate = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 0, 0, 0);

                    else if (scheduler.isFuture && scheduler.NextExecutionDateTime != null)
                        _startDate = (DateTime)scheduler.NextExecutionDateTime;

                    if (_endDate >= currentDateTime) _endDate = currentDateTime.AddDays(-1); // if endDate cross the CurrentDate; then endDate would be change 

                    if (_endDate.Date < _startDate.Date) _endDate = _startDate;  

                    switch (scheduler.SchedulerName)
                    {
                        case "HcsReportManagementSummary":
                            response = await _reportSummary.ReportManagementSummaryAsync(_startDate, _endDate);
                            flag = response.IsSuccess;
                            break;
                        case "HcsBIRatePlanStatisticsHistory":
                            response = await _hcsBIRatePlanStatisticsHistoryService.HcsBIRatePlanStatisticsRepositoryHistoryAsyc(_startDate, _endDate);
                            flag = response.IsSuccess;
                            break;
                        case "HcsBIRatePlanStatisticsFuture":
                            response = await _hcsBIRatePlanStatisticsFutureService.HcsBIRatePlanStatisticsRepositoryFutureAsyc(_startDate, scheduler.DaysLimit);
                            flag = response.IsSuccess;
                            break;
                        case "HcsBIReservationDashboardHistory":
                            response = await _hcsBIReservationDashboardHistoryService.HcsBIReservationDashboardRepositoryAsyc(_startDate, _endDate);
                            flag = response.IsSuccess;
                            break;
                        case "HcsBIReservationDashboardFuture":
                            response = await _hcsBIReservationDashboardFutureService.HcsBIReservationDashboardRepositoryAsyc(_startDate, scheduler.DaysLimit);
                            flag = response.IsSuccess;
                            break;
                        case "HcsBISourceStatisticsHistory":
                            response = await _hcsBISourceStatisticsHistoryService.HcsBIHcsBISourceStatisticsRepositoryHistoryAsyc(_startDate, _endDate);
                            flag = response.IsSuccess;
                            break;

                        case "HcsBISourceStatisticsFuture":
                            response = await _hcsBISourceStatisticsFutureService.HcsBIHcsBISourceStatisticsRepositoryFutureAsyc(_startDate, scheduler.DaysLimit);
                            flag = response.IsSuccess;
                            break;
                        case "HcsGetDailyHistory":
                            response = await _hcsGetDailyHistoryService.HcsGetDailyHistoryAsyc(_startDate, _endDate);
                            flag = response.IsSuccess;
                            break;

                        case "HcsGetDailyHistoryFuture":
                            response = await _hcsGetDailyFutureService.HcsGetDailyHistoryFutureAsyc(_startDate, scheduler.DaysLimit);
                            flag = response.IsSuccess;
                            break;

                        default:
                            break;
                    }
                    DateTime? dateTime = null;
                    dtos.LastExecutionDateTime = DateTime.UtcNow;
                    dtos.NextExecutionDateTime = DateTime.UtcNow.AddMinutes(scheduler.ExecutionIntervalMins);
                    dtos.LastBusinessDate = scheduler.isFuture == false ? _endDate.Date : dateTime; //_Future does not need LastBusinessDate, because tartingpoint is always To
                    dtos.SchedulerName = scheduler.SchedulerName;
                   
                    if (flag)
                    {
                        await _schedulerSetupService.SaveWithProcAsync(dtos);
                        await _schedulerLogService.SaveWithProcAsync(scheduler.SchedulerName, logDayLimits);
                    }

                }

            }
        }
    }
}
