using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Enums;
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
        private readonly IHcsGetDailyHistoryHistoryService _hcsGetDailyHistoryHistoryService;
        private readonly IHcsGetDailyFutureService _hcsGetDailyFutureService;
        private readonly IHcsGetFullReservationDetailsService _hcsGetFullReservationDetailsService;
        private readonly IHcsListMealPlansService _hcsListMealPlans;
        private readonly IHcsListBanquetingRoomsService _hcsListBanquetingRoomsService;
        private readonly IHcsListRoomService _hcsRoomsService;
        private readonly IHcsListPackageService _hcsListPackagesService;
        private readonly IHcsListRateTypesService _hcsListRateTypesService;
        private readonly IHcsListSubSourcesService _hcsListSubSourcesService;
        private readonly IHcsListSourcesService _hcsListSourcesService;
        private readonly IHcsListRoomTypeService _hcsListRoomTypeService;
        private readonly IHcsListBanquetingRoomTypesService _hcsListBanquetingRoomTypesService;
        private readonly IHcsListStatisticsRevenueCodeService _hcsListStatisticsRevenueCodeService;
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
            IHcsGetDailyHistoryHistoryService hcsGetDailyHistoryService,
            IHcsGetDailyFutureService hcsGetDailyFutureService,
            IHcsGetFullReservationDetailsService hcsGetFullReservationDetailsService,
            IHcsListMealPlansService hcsListMealPlans,
            IHcsListBanquetingRoomsService hcsListBanquetingRoomsService,
            IHcsListRoomService hcsRoomsService,
            IHcsListPackageService hcsListPackagesService,
            IHcsListRateTypesService hcsListRateTypesService,
            IHcsListSubSourcesService hcsListSubSourcesService,
            IHcsListSourcesService hcsListSourcesService,
            IHcsListRoomTypeService hcsListRoomTypeService,
            IHcsListBanquetingRoomTypesService hcsListBanquetingRoomTypesService,
            IHcsListStatisticsRevenueCodeService hcsListStatisticsRevenueCodeService,

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
            _hcsGetDailyHistoryHistoryService = hcsGetDailyHistoryService;
            _hcsGetDailyFutureService = hcsGetDailyFutureService;
            _hcsGetFullReservationDetailsService = hcsGetFullReservationDetailsService;
            _hcsListMealPlans = hcsListMealPlans;
            _hcsListBanquetingRoomsService = hcsListBanquetingRoomsService;
            _hcsRoomsService = hcsRoomsService;
            _hcsListPackagesService = hcsListPackagesService;
            _hcsListRateTypesService = hcsListRateTypesService;
            _hcsListSubSourcesService = hcsListSubSourcesService;
            _hcsListSourcesService = hcsListSourcesService;
            _hcsListRoomTypeService = hcsListRoomTypeService;
            _hcsListBanquetingRoomTypesService = hcsListBanquetingRoomTypesService;
            _hcsListStatisticsRevenueCodeService = hcsListStatisticsRevenueCodeService;
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
                        case "HcsGetDailyHistoryHistory":
                            response = await _hcsGetDailyHistoryHistoryService.HcsGetDailyHistoryHistoryAsyc(_startDate, _endDate);
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
                            response = await _hcsRoomsService.HcsListRoomsServiceAsync();
                            flag = response.IsSuccess;
                            break;

                        case "HcsListPackages":
                            response = await _hcsListPackagesService.HcsListPackagesServiceeAsync();
                            flag = response.IsSuccess;
                            break;

                        case "HcsListRateTypes":
                            response = await _hcsListRateTypesService.HcsListRateTypesAsync();
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
                        case "HcsListStatisticsRevenueCodes":
                            response = await _hcsListStatisticsRevenueCodeService.HcsListStatisticsRevenueCodeAsyc();
                            flag = response.IsSuccess;
                            break;
                        case "HcsListRoomTypes":
                            response = await _hcsListRoomTypeService.HcsListRoomTypeAsync();
                            flag = response.IsSuccess;
                            break;
                        case "HcsListBanquetingRoomTypes":
                            response = await _hcsListBanquetingRoomTypesService.HcsListBanquetingRoomTypesAsync();
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
                        dtos.SchedulerStatus = SchedulerStatus.Succeed.ToString();
                        await _schedulerSetupService.SaveWithProcAsync(dtos);
                        await _schedulerLogService.SaveWithProcAsync(scheduler.SchedulerName, logDayLimits);
                    }

                }

            }
        }

        public async Task ResetStatusAsync()
        {
           await _schedulerSetupService.ResetStatusAsync();
        }
    }
}
