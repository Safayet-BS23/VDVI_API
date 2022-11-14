using Framework.Core.Repository;
using VDVI.Repository.DbContext.ApmaDbContext;
using VDVI.Repository.ApmaRepository.Implementation;
using VDVI.ApmaRepository.Interfaces; 

namespace VDVI.ApmaRepository
{
    public class MasterRepository : ProRepository, IMasterRepository
    {

        private readonly VDVISchedulerDbContext _dbContext;

        public MasterRepository(VDVISchedulerDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        // Common
        public ISchedulerSetupRepository SchedulerSetupRepository => new SchedulerSetupRepository(_dbContext);
        public ISchedulerLogRepository SchedulerLogRepository => new SchedulerLogRepository(_dbContext);

        // Accounts
        public IHcsLedgerBalanceHistoryRepository HcsLedgerBalanceRepository => new HcsLedgerBalanceHistoryRepository(_dbContext);
        public IHcsBIRevenueHistoryRepository HcsBIRevenueHistoryRepository => new HcsBIRevenueHistoryRepository(_dbContext);
        public IHcsBIRevenueFutureRepository HcsBIRevenueFutureRepository => new HcsBIRevenueFutureRepository(_dbContext);
        public IHcsBIRevenueFutureAuditRepository HcsBIRevenueFutureAuditRepository => new HcsBIRevenueFutureAuditRepository(_dbContext);
        public IHcsBIRatePlanStatisticsHistoryRepository HcsBIRatePlanStatisticsHistoryRepository => new HcsBIRatePlanStatisticsHistoryRepository(_dbContext);
        public IHcsBIRatePlanStatisticsFutureRepository HcsBIRatePlanStatisticsFutureRepository => new HcsBIRatePlanStatisticsFutureRepository(_dbContext);
        public IHcsBIRatePlanStatisticsFutureAuditRepository HcsBIRatePlanStatisticsFutureAuditRepository => new HcsBIRatePlanStatisticsFutureAuditRepository(_dbContext);
        public IHcsListRateTypeRepository HcsListRateTypeRepository => new HcsListRateTypeRepository(_dbContext);

        // RoomSummary
        public IHcsRoomSummaryHistoryRepository HcsRoomSummaryHistoryRepository => new HcsRoomSummaryHistoryRepository(_dbContext);
        public IHcsBIOccupancyHistoryRepository HcsBIOccupancyHistoryRepository => new HcsBIOccupancyHistoryRepository(_dbContext);
        public IHcsBIOccupancyFutureRepository HcsBIOccupancyFutureRepository => new HcsBIOccupancyFutureRepository(_dbContext);
        public IHcsBIOccupancyFutureAuditRepository HcsBIOccupancyFutureAuditRepository => new HcsBIOccupancyFutureAuditRepository(_dbContext);
        public IHcsBIRoomsHistoryRepository HcsBIRoomsHistoryRepository => new HcsBIRoomsHistoryRepository(_dbContext);
        public IHcsBIRoomsFutureRepository HcsBIRoomsFutureRepository => new HcsBIRoomsFutureRepository(_dbContext);
        public IHcsBIRoomsFutureAuditRepository HcsBIRoomsFutureAuditRepository => new HcsBIRoomsFutureAuditRepository(_dbContext);
        public IHcsBIReservationHistoryRepository HcsBIReservationHistoryRepository => new HcsBIReservationHistoryRepository(_dbContext);
        public IHcsBIReservationFutureRepository HcsBIReservationFutureRepository => new HcsBIReservationFutureRepository(_dbContext);
        public IHcsBIReservationFutureAuditRepository HcsBIReservationFutureAuditRepository => new HcsBIReservationFutureAuditRepository(_dbContext);
        public IHcsDailyHistoryRepository HcsDailyHistoryRepository => new HcsDailyHistoryRepository(_dbContext);
        public IHcsDailyHistoryFutureRepository HcsDailyHistoryFutureRepository =>   new HcsDailyHistoryFutureRepository(_dbContext);
        public IHcsDailyFutureAuditRepository HcsDailyHistoryFutureAuditRepository =>  new HcsDailyHistoryFutureAuditRepository(_dbContext);
        public IHcsGetFullReservationDetailsRepository HcsGetFullReservationDetailsRepository => new HcsGetFullReservationDetailsRepository(_dbContext);
        public IHcsBanquetingRoomsRepository HcsBanquetingRoomsRepository => new HcsBanquetingRoomsRepository(_dbContext);


        // Source
        public IHcsBISourceStatisticsHistoryRepository HcsBISourceStatisticsHistoryRepository => new HcsBISourceStatisticsHistoryRepository(_dbContext);
        public IHcsBISourceStatisticsFutureRepository HcsBISourceStatisticsFutureRepository => new HcsBISourceStatisticsFutureRepository(_dbContext);
        public IHcsBISourceStatisticsFutureAuditRepository HcsBISourceStatisticsFutureAuditRepository => new HcsBISourceStatisticsFutureAuditRepository(_dbContext); 
        public IHcsListSourcesRepository HcsListSourcesRepository => new HcsListSourcesRepository(_dbContext); 
        public IHcsListSubSourcesRepository HcsListSubSourcesRepository => new HcsListSubSourcesRepository(_dbContext); 
        public IHcsListPackagesRepository HcsListPackagesRepository => new HcsListMealPlansRepository(_dbContext);
    }
}