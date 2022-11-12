using Unity;
using Unity.Lifetime;
using Framework.Core.Repository;
using VDVI.Repository.DbContext.ApmaDbContext;
using VDVI.Services.Interfaces;
using Framework.Core.IoC;
using VDVI.Services;
using VDVI.ApmaRepository;
using VDVI.Services.Services.ApmaServices;
using VDVI.Services.Interfaces.APMA;
using VDVI.Services.APMA;
using VDVI.Repository.DbContext.AfasDbContext;
using VDVI.Services.Interfaces.AfasInterfaces.Administrators;
using VDVI.Services.AfasInterfaces;
using VDVI.Services.AfasServices;
using VDVI.AfasRepository;
using VDVI.Services.AFAS;
using VDVI.Services.Interfaces.AFAS; 

namespace VDVI.Client.IoC
{
    public class UnityDependencyProvider : IDependencyProvider
    {
        public void RegisterDependencies(IUnityContainer container)
        { 
             
            //APMA-Db Context         
            container.RegisterType<IVDVISchedulerDbContext, VDVISchedulerDbContext>(new SingletonLifetimeManager());

            //AFAS-DbContext
            container.RegisterType<IAfasDbContext, AfasDbContext>(new SingletonLifetimeManager());

            container.RegisterType<IProRepository, ProRepository>();     
            
            //APMA-MASTER
            container.RegisterType<IMasterRepository, MasterRepository>();
          
            //APMA-Scheduler
            container.RegisterType<IApmaTaskSchedulerService, ApmaTaskSchedulerService>();
            container.RegisterType<ISchedulerSetupService, SchedulerSetupService>(); 
            container.RegisterType<ISchedulerLogService, SchedulerLogService>();

            //Parent-APMA
            container.RegisterType<IHcsReportManagementSummaryService, HcsReportManagementSummaryService>();
            container.RegisterType<IHcsBIReservationDashboardHistoryService, HcsBIReservationDashboardHistoryService>();
            container.RegisterType<IHcsBIReservationDashboardFutureService, HcsBIReservationDashboardFutureService>();
            container.RegisterType<IHcsBIRatePlanStatisticsHistoryService, HcsBIRatePlanStatisticsHistoryService>();
            container.RegisterType<IHcsBIRatePlanStatisticsFutureService, HcsBIRatePlanStatisticsFutureService>();            
            container.RegisterType<IHcsBISourceStatisticsHistoryService, HcsBISourceStatisticsHistoryService>();
            container.RegisterType<IHcsBISourceStatisticsFutureService, HcsBISourceStatisticsFutureService>();
            container.RegisterType<IHcsGetDailyHistoryService, HcsGetDailyHistoryService>();
            container.RegisterType<IHcsRoomSummaryService, HcsRoomSummaryService>();
            container.RegisterType<IHcsLedgerBalanceService, HcsLedgerBalanceService>();
            container.RegisterType<IHcsGetDailyFutureService, HcsGetDailyHistoryFutureService>(); 
            container.RegisterType<IHcsGetFullReservationDetailsService, HcsGetFullReservationDetailsService>(); 
            container.RegisterType<IHcsListMealPlansService, HcsListMealPlansService>(); 
            container.RegisterType<IHcsListBanquetingRoomsService, HcsListBanquetingRoomsService>(); 
            container.RegisterType<IHcsListSubSourcesService, HcsListSubSourcesService>(); 
            container.RegisterType<IHcsListSourcesService, HcsListSourcesService>(); 
            container.RegisterType<IHcsListRoomsService, HcsListRoomsService>(); 
            container.RegisterType<IHcsListPackagesService, HcsListPackagesService>();  
            container.RegisterType<IHcsListRateTypesService, HcsListRateTypesService>(); 


            //Child-APMA
            container.RegisterType<IHcsRatePlanStatisticsHistoryService, HcsRatePlanStatisticsHistoryService>();
            container.RegisterType<IHcsSourceStasticsHistoryService, HcsSourceStasticsHistoryService>();            
            container.RegisterType<IHcsBIOccupancyHistoryService, HcsBIOccupancyHistoryService>();
            container.RegisterType<IHcsBIOccupancyFutureService, HcsBIOccupancyFutureService>();
            container.RegisterType<IHcsBIReservationHistoryService, HcsBIReservationHistoryService>();
            container.RegisterType<IHcsBIReservationFutureService, HcsBIReservationFutureService>();
            container.RegisterType<IHcsBIRoomsHistoryService, HcsBIRoomsHistoryService>();
            container.RegisterType<IHcsBIRoomsFutureService, HcsBIRoomsFutureService>();
            container.RegisterType<IHcsBIRevenueHistoryService, HcsBIRevenueHistoryService>();
            container.RegisterType<IHcsBIRevenueFutureService, HcsBIRevenueFutureService>();
            container.RegisterType<IHcsDailyHistoryService, HcsDailyHistoryService>();   
            container.RegisterType<IHcsSourceStasticsFutureService, HcsSourceStasticsFutureService>();
            container.RegisterType<IHcsRatePlanStatisticsFutureService, HcsRatePlanStatisticsFutureService>();
            container.RegisterType<IHcsDailyFutureService, HcsDailyHistoryFutureService>(); 
            container.RegisterType<IHcsGetFullReservationDetailService, HcsGetFullReservationDetailService>(); 
            container.RegisterType<IHcsBanquetingRoomService, HcsBanquetingRoomService>();  
            container.RegisterType<IHcsListSourceService, HcsListSourceService>();  
            container.RegisterType<IHcsListSubSourceService, HcsListSubSourceService>();  
            container.RegisterType<IHcsListRateTypeService, HcsListRateTypeService>();  
          
            //AFAS-Master
            container.RegisterType<IAfasMasterRepositroy, AfasMasterRepository>();

            //AFAS-Schedulers
            container.RegisterType<IAfasTaskSchedulerService, AfasTaskSchedulerService>();
            container.RegisterType<IAfasSchedulerSetupService, AfasSchedulerSetupService>();
            container.RegisterType<IAfasSchedulerLogService, AfasSchedulerLogService>(); 

            //Parent-AFAS
            container.RegisterType<IdmfAdministratiesService, DmfAdministratiesService>();
            container.RegisterType<IdmfBeginbalaniesService, DmfBeginbalaniesService>();
            container.RegisterType<IdmfGrootboekrekeningen, DmfGrootboekrekeningensService>();
            container.RegisterType<IdmfGrootboekrekeningen, DmfGrootboekrekeningensService>();
            container.RegisterType<IdmfFinancieleMutatiesService, DmfFinancieleMutatesService>();
            container.RegisterType<IdmfBoekingsdagenMutatiesService, DmfBoekingsdagenMutatesService>();

            //Child-AFAS
            container.RegisterType<IdmfAdministraterService, DmfAdministraterService>();
            container.RegisterType<IdmfBeginbalansService, DMFBeginbalansService>();
            container.RegisterType<IdmfGrootboekrekeningenService, DMFGrootboekrekeningenService>(); 
            container.RegisterType<IdmFFinancieleMutationService, DMFFinancieleMutationService>();
            container.RegisterType<IdmFBoekingsdagenMutationService, DMFBoekingsdagenMutationService>();


        }
    }
}
