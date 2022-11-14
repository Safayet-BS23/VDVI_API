using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.DbContext;
using VDVI.DB.Dtos;
using VDVI.Repository.DB; 

namespace VDVI.Repository.DbContext.ApmaDbContext
{
    public interface IVDVISchedulerDbContext : IDapperDbContext
    {
        IDapperRepository<DbSchedulerSetup> SchedulerSetup { get; }
        IDapperRepository<DbSchedulerLog> SchedulerLog { get; }
        IDapperRepository<DbRoomSummaryHistory> RoomSummary { get; }
        IDapperRepository<DbLedgerBalanceHistory> LedgerBalance { get; }
        IDapperRepository<DbSourceStatisticHistory> SourceStatistic { get; }
        IDapperRepository<DbSourceStatisticFuture> SourceStatisticFuture { get; }
        IDapperRepository<DbSourceStatisticFutureAudit> SourceStatisticFutureAudit { get; }
        IDapperRepository<DbRatePlanStatisticHistory> RatePlanStatisticHistory { get; }
        IDapperRepository<DbReservationDashboardRoomsHistory> ReservationDashboardRooms { get; }
        IDapperRepository<DbReservationDashboardRevenueHistory> Revenue { get; }
        IDapperRepository<DbReservationDashboardOccupancyHistory> Occupancy { get; }
        IDapperRepository<DbReservationDashboardReservationHistory> Reservation { get; }
        IDapperRepository<DbDailyHistory> DailyHistory { get; }
        IDapperRepository<DbGetFullReservationDetails> GetFullReservationDetails { get; }
        IDapperRepository<DbBanquetingRooms> BanquetingRooms { get; }
        IDapperRepository<DbSubSources> SubSources { get; }
        IDapperRepository<DbRooms> Rooms { get; }
        
    }
}
