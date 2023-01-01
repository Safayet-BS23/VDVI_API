using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.DB.Dtos;
using VDVI.Services.Interfaces;
using VDVI.Services.Interfaces.APMA;
using VDVI.Services.MediatR.Models;
using VDVI.Services.Rebus.Models;
using Serilog;

namespace VDVI.Services.MediatR.Services.Apma
{
    public class ApmaEventService : IApmaEventService
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
        private readonly ISchedulerLogService _schedulerLogService;
        private readonly IHcsListGroupReservationService _hcsListGroupReservationService;
        private readonly IHcsListFolioDetailService _hcsListFolioDetailService;

        public ApmaEventService(
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
            ISchedulerLogService schedulerLogService,
            IHcsListGroupReservationService hcsListGroupReservationService,
            IHcsListFolioDetailService hcsListFolioDetailService
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
            _hcsListGroupReservationService = hcsListGroupReservationService;
            _hcsListFolioDetailService = hcsListFolioDetailService;
        }

        public async Task ExecuteEventAsync(ApmaSchedulerEvent schedulerEvent)
        {
            Log.Information($"Step-1=>>APMA: APMA Scheduler object: " + schedulerEvent.Scheduler.SchedulerName + " NextExTime:-" + schedulerEvent.Scheduler.NextExecutionDateTime + " Current Data Time:-" + DateTime.UtcNow);

            Result<PrometheusResponse> response;
            bool flag = false;

            schedulerEvent.StartDate = schedulerEvent.StartDate.HasValue ? schedulerEvent.StartDate.Value : DateTime.UtcNow;
            schedulerEvent.EndDate = schedulerEvent.EndDate.HasValue ? schedulerEvent.EndDate.Value : DateTime.UtcNow;

            int daysLimit = schedulerEvent.DaysLimit.HasValue ? schedulerEvent.DaysLimit.Value : 0;

            SchedulerSetupDto dtos = new SchedulerSetupDto();


            switch (schedulerEvent.Scheduler.SchedulerName)
            {
                case "HcsReportManagementSummary":
                    response = await _reportSummary.ReportManagementSummaryAsync((DateTime)schedulerEvent.StartDate, (DateTime)schedulerEvent.EndDate);
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "HcsReportManagementSummary";
                    break;

                case "HcsBIRatePlanStatisticsHistory":
                    response = await _hcsBIRatePlanStatisticsHistoryService.HcsBIRatePlanStatisticsRepositoryHistoryAsyc((DateTime)schedulerEvent.StartDate, (DateTime)schedulerEvent.EndDate);
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "HcsBIRatePlanStatisticsHistory";
                    break;

                case "HcsBIRatePlanStatisticsFuture":
                    response = await _hcsBIRatePlanStatisticsFutureService.HcsBIRatePlanStatisticsRepositoryFutureAsyc((DateTime)schedulerEvent.StartDate, daysLimit);
                    flag = response.IsSuccess;                     
                    dtos.SchedulerName = "HcsBIRatePlanStatisticsFuture";
                    break;

                case "HcsBIReservationDashboardHistory":
                    response = await _hcsBIReservationDashboardHistoryService.HcsBIReservationDashboardRepositoryAsyc((DateTime)schedulerEvent.StartDate, (DateTime)schedulerEvent.EndDate);
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "HcsBIReservationDashboardHistory";
                    break;

                case "HcsBIReservationDashboardFuture":
                    response = await _hcsBIReservationDashboardFutureService.HcsBIReservationDashboardRepositoryAsyc((DateTime)schedulerEvent.StartDate, daysLimit);
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "HcsBIReservationDashboardFuture";
                    break;

                case "HcsBISourceStatisticsHistory":
                    response = await _hcsBISourceStatisticsHistoryService.HcsBIHcsBISourceStatisticsRepositoryHistoryAsyc((DateTime)schedulerEvent.StartDate, (DateTime)schedulerEvent.EndDate);
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "HcsBISourceStatisticsHistory";
                    break;

                case "HcsBISourceStatisticsFuture":
                    response = await _hcsBISourceStatisticsFutureService.HcsBIHcsBISourceStatisticsRepositoryFutureAsyc((DateTime)schedulerEvent.StartDate, daysLimit);
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "HcsBISourceStatisticsFuture";
                    break;
                case "HcsGetDailyHistoryHistory":
                    //response = await _hcsGetDailyHistoryHistoryService.HcsGetDailyHistoryHistoryAsyc((DateTime)schedulerEvent.StartDate, (DateTime)schedulerEvent.EndDate);
                    response = await _hcsGetDailyHistoryHistoryService.HcsGetDailyHistoryHistoryAsyc((DateTime)schedulerEvent.Scheduler.BusinessStartDate);
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "HcsGetDailyHistoryHistory";
                    break;

                case "HcsGetDailyHistoryFuture":
                    response = await _hcsGetDailyFutureService.HcsGetDailyHistoryFutureAsyc((DateTime)schedulerEvent.StartDate, daysLimit);
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "HcsGetDailyHistoryFuture";
                    break;

                case "HcsGetFullReservationDetails":
                    response = await _hcsGetFullReservationDetailsService.HcsGetFullReservationDetailsAsync();
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "HcsGetFullReservationDetails";
                    break;

                case "HcsListMealPlans":
                    response = await _hcsListMealPlans.HcsListMealPlansAsync();
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "HcsListMealPlans";
                    break;

                case "HcsListBanquetingRooms":
                    response = await _hcsListBanquetingRoomsService.HcsListBanquetingRoomsAsync();
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "HcsListBanquetingRooms";
                    break;

                case "HcsListRooms":
                    response = await _hcsRoomsService.HcsListRoomsServiceAsync();
                    flag = response.IsSuccess; 
                    dtos.SchedulerName = "HcsListRooms";
                    break;

                case "HcsListPackages":
                    response = await _hcsListPackagesService.HcsListPackagesServiceeAsync();
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "HcsListPackages";
                    break;

                case "HcsListRateTypes":
                    response = await _hcsListRateTypesService.HcsListRateTypesAsync();
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "HcsListRateTypes";
                    break;

                case "HcsListSubSources":
                    response = await _hcsListSubSourcesService.HcsListSubSourcesServiceAsync();
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "HcsListSubSources";
                    break;

                case "HcsListSources":
                    response = await _hcsListSourcesService.HcsListSourcesServiceAsync();
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "HcsListSources";
                    break;
                case "HcsListStatisticsRevenueCodes":
                    response = await _hcsListStatisticsRevenueCodeService.HcsListStatisticsRevenueCodeAsyc();
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "HcsListStatisticsRevenueCodes";
                    break;
                case "HcsListRoomTypes":
                    response = await _hcsListRoomTypeService.HcsListRoomTypeAsync();
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "HcsListRoomTypes";
                    break;

                case "HcsListBanquetingRoomTypes":
                    response = await _hcsListBanquetingRoomTypesService.HcsListBanquetingRoomTypesAsync();
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "HcsListBanquetingRoomTypes";
                    break;

                case "HcsGroupReservation":
                    response = await _hcsListGroupReservationService.HcsGetGroupReservationsAsync((DateTime)schedulerEvent.StartDate, (DateTime)schedulerEvent.EndDate);
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "HcsGroupReservation";
                    break;

                case "HcsGetFolioDetails":
                    response = await _hcsListFolioDetailService.HcsListFolioDetailAsync((DateTime) schedulerEvent.Scheduler.BusinessStartDate);
                    flag = response.IsSuccess;
                    dtos.SchedulerName = "HcsGetFolioDetails";
                    break;

                default:
                    break;

            }
            Log.Information($"Step-2=>>Apma: Apma Scheduler Name: " + schedulerEvent.Scheduler.SchedulerName + " NextExTime:-" + schedulerEvent.Scheduler.NextExecutionDateTime + " Current UTC TIME:-" + DateTime.UtcNow);

            if (schedulerEvent.Scheduler.SchedulerName == dtos.SchedulerName && flag)
            {
                Log.Information($"Step-3=> After finished business implementations: " + dtos.SchedulerName + " Current UTC TIME:-" + DateTime.UtcNow);

                //DateTime? dateTime = null;
                //dtos.LastExecutionDateTime = DateTime.UtcNow;
                //dtos.NextExecutionDateTime = schedulerEvent.Scheduler.NextExecutionDateTime.Value.AddMinutes(schedulerEvent.Scheduler.ExecutionIntervalMins); //NextExecutionDateTime=NextExecutionDateTime+ExecutionIntervalMins
                //dtos.LastBusinessDate = schedulerEvent.Scheduler.isFuture == false ? (DateTime)schedulerEvent.EndDate.Date : dateTime; //_Future does not need LastBusinessDate, because tartingpoint is always To

                //dtos.SchedulerStatus = SchedulerStatus.Succeed.ToString();
                var res = await _schedulerSetupService.FindByMethodNameAsync(schedulerEvent.Scheduler.SchedulerName);

                Log.Information($"Step-4=>>Apma: Apma Scheduler Log Save Before: " + schedulerEvent.Scheduler.SchedulerName + " NextExTime:-" + res.NextExecutionDateTime + " Current UTC TIME:-" + DateTime.UtcNow);
                await _schedulerSetupService.SaveWithProcAsync(schedulerEvent);

                dtos = new SchedulerSetupDto();
                flag = false;
                Log.Information($"Step-7=>>Apma-Refesh all object" + " Current UTC TIME:-" + DateTime.UtcNow);

            }
        }
    }
}
