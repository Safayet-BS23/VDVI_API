using CSharpFunctionalExtensions;
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
        private readonly IHcsGetFullReservationDetailsService _hcsGetFullReservationDetailsService;
        private readonly IHcsListMealPlansService _hcsListMealPlans;
        private readonly IHcsListBanquetingRoomsService _hcsListBanquetingRoomsService;
        private readonly IHcsListRoomsService _hcsListRoomsService;
        private readonly IHcsListPackagesService _hcsListPackagesService;
        private readonly IHcsListRateTypeService _hcsListRateTypeService;
        private readonly IHcsListSubSourcesService _hcsListSubSourcesService;
        private readonly IHcsListSourcesService _hcsListSourcesService;
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
            IHcsBISourceStatisticsHistoryService hcsBISourceStatisticsHistoryService,
            IHcsBISourceStatisticsFutureService hcsBISourceStatisticsFutureService,
            IHcsGetDailyHistoryService hcsGetDailyHistoryService,
            IHcsGetDailyFutureService hcsGetDailyFutureService,
            IHcsGetFullReservationDetailsService hcsGetFullReservationDetailsService,
            IHcsListMealPlansService hcsListMealPlans,
            IHcsListBanquetingRoomsService hcsListBanquetingRoomsService,
            IHcsListRoomsService hcsListRoomsService,
            IHcsListPackagesService hcsListPackagesService,
            IHcsListRateTypeService hcsListRateTypeService,
            IHcsListSubSourcesService hcsListSubSourcesService,
            IHcsListSourcesService hcsListSourcesService,

            ISchedulerSetupService schedulerSetupService,
            ISchedulerLogService schedulerLogService


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
            _hcsGetFullReservationDetailsService = hcsGetFullReservationDetailsService;
            _hcsListMealPlans = hcsListMealPlans;
            _hcsListBanquetingRoomsService = hcsListBanquetingRoomsService;

            _hcsListRoomsService = hcsListRoomsService;
            _hcsListPackagesService = hcsListPackagesService;
            _hcsListRateTypeService = hcsListRateTypeService;
            _hcsListSubSourcesService = hcsListSubSourcesService;
            _hcsListSourcesService = hcsListSourcesService;
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
            var logDayLimits = Convert.ToInt32(_config.GetSection("SchedulerLog").GetSection("APMASchedulerLogLimitDays").Value);

            var schedulers = await _schedulerSetupService.FindByAllScheduleAsync();
            var new1 = schedulers.ToList();

            for (int i = 0; i < new1.Count(); i++)
            {
                var scheduler = new1[i];

                // LastBusinessDate marked to NextExecutionDate 
                //if (scheduler.LastBusinessDate != null)
                //    scheduler.LastBusinessDate = ((DateTime)scheduler.LastBusinessDate).AddDays(1);

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

                        case "HcsGetFullReservationDetails":
                            response = await _hcsGetFullReservationDetailsService.HcsGetFullReservationDetailsAsync();
                            flag = response.IsSuccess;
                            break;

                        case "HcsListMealPlans":
                            response = await _hcsListMealPlans.HcsListMealPlansAsync();
                            flag = response.IsSuccess;
                            break;

                        case "HcsListBanquetingRooms":
                            response = await _hcsListBanquetingRoomsService.HcsListBanquetingRoomsAsync();
                            flag = response.IsSuccess;
                            break;

                        case "HcsListRooms":
                            response = await _hcsListRoomsService.HcsListRoomsServiceAsync();
                            flag = response.IsSuccess;
                            break;

                        case "HcsListPackages":
                            response = await _hcsListPackagesService.HcsListPackagesServiceeAsync();
                            flag = response.IsSuccess;
                            break;

                        case "HcsListRateType":
                            response = await _hcsListRateTypeService.HcsListRateTypeAsync();
                            flag = response.IsSuccess;
                            break; 

                        case "HcsListSubSources":
                            response = await _hcsListSubSourcesService.HcsListSubSourcesServiceAsync();
                            flag = response.IsSuccess;
                            break;

                        case "HcsListSources":
                            response = await _hcsListSourcesService.HcsListSourcesServiceAsync();
                            flag = response.IsSuccess;
                            break;

                        default:
                            break;

                    }
                    DateTime? dateTime = null;
                    dtos.LastExecutionDateTime = currentDateTime;
                    dtos.NextExecutionDateTime = scheduler.NextExecutionDateTime.Value.AddMinutes(scheduler.ExecutionIntervalMins); //NextExecutionDateTime=NextExecutionDateTime+ExecutionIntervalMins
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
