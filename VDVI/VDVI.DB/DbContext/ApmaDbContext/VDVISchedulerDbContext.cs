using Framework.Core.Repository;
using MicroOrm.Dapper.Repositories;
using Microsoft.Extensions.Configuration;
using Nelibur.ObjectMapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using VDVI.DB.Dtos;
using VDVI.Repository.DB; 

namespace VDVI.Repository.DbContext.ApmaDbContext
{

    public class VDVISchedulerDbContext : ProDbContext, IVDVISchedulerDbContext
    {
        private IDapperRepository<DbSchedulerSetup> _schedulerSetup;
        private IDapperRepository<DbSchedulerLog> _schedulerlog;

        private IDapperRepository<DbRoomSummaryHistory> _roomSummary;
        private IDapperRepository<DbLedgerBalanceHistory> _ledgerBalance;

        private IDapperRepository<DbRatePlanStatisticHistory> _ratePlanStatistic;
        private IDapperRepository<DbSourceStatisticHistory> _sourceStatistic;
        private IDapperRepository<DbReservationDashboardOccupancyHistory> _occupancy;
        private IDapperRepository<DbReservationDashboardReservationHistory> _reservation;
        private IDapperRepository<DbReservationDashboardRoomsHistory> _rooms;
        private IDapperRepository<DbReservationDashboardRevenueHistory> _revenue;
        private IDapperRepository<DbSourceStatisticFuture> _sourceStatisticFuture;
        private IDapperRepository<DbSourceStatisticFutureAudit> _sourceStatisticFutureAudit;
        private IDapperRepository<DbRatePlanStatisticFuture> _ratePlanStatisticFuture;
        private IDapperRepository<DbReservationDashboardRevenueFuture> _revenueFuture;
        private IDapperRepository<DbReservationDashboardRoomsFuture> _roomsFuture;
        private IDapperRepository<DbReservationDashboardReservationFuture> _reservationFuture;
        private IDapperRepository<DbReservationDashboardOccupancyFuture> _occupancyFuture;
        private IDapperRepository<DbRatePlanStatisticFutureAudit> _ratePlanStatisticFutureAudit;
        private IDapperRepository<DbReservationDashboardRoomsFutureAudit> _roomsFutureAudit;
        private IDapperRepository<DbReservationDashboardOccupancyFutureAudit> _occupancyFutureAudit;
        private IDapperRepository<DbReservationDashboardReservationFutureAudit> _reservationFutureAudit;
        private IDapperRepository<DbReservationDashboardRevenueFutureAudit> _revenueFutureAudit;
        private IDapperRepository<DbDailyHistory> _dailyHistory;
        private IDapperRepository<DbGetFullReservationDetails> _getFullReservationDetails;
        private IDapperRepository<DbBanquetingRooms> _banquetingRooms;
        private IDapperRepository<DbSources> _sources;
        private IDapperRepository<DbSubSources> _subsources;





        public VDVISchedulerDbContext(IConfiguration configuration, IDapperRepository<DbGetFullReservationDetails> getFullReservationDetails = null) : base(new SqlConnection(configuration["ConnectionStrings:ApmaDb"]))
        {
            // Dto to Db - Single
            TinyMapper.Bind<SchedulerSetupDto, DbSchedulerSetup>();
            TinyMapper.Bind<SchedulerLogDto, DbSchedulerLog>();
            TinyMapper.Bind<ReservationDashboardRoomSummaryHistoryDto, DbRoomSummaryHistory>();
            TinyMapper.Bind<LedgerBalanceHistoryDto, DbLedgerBalanceHistory>();
            TinyMapper.Bind<RatePlanStatisticHistoryDto, DbRatePlanStatisticHistory>();
            TinyMapper.Bind<SourceStatisticHistoryDto, DbSourceStatisticHistory>();
            TinyMapper.Bind<ReservationDashboardOccupancyHistoryDto, DbReservationDashboardOccupancyHistory>();
            TinyMapper.Bind<ReservationDashboardReservationHistoryDto, DbReservationDashboardReservationHistory>();
            TinyMapper.Bind<ReservationDashboardRoomsHistoryDto, DbReservationDashboardRoomsHistory>();
            TinyMapper.Bind<ReservationDashboardRevenueHistoryDto, DbReservationDashboardRevenueHistory>();
            TinyMapper.Bind<SourceStatisticFutureDto, DbSourceStatisticFuture>();
            TinyMapper.Bind<SourceStatisticsFutureAuditDto, DbSourceStatisticFutureAudit>();
            TinyMapper.Bind<ReservationDashboardOccupancyFutureDto, DbReservationDashboardOccupancyFuture>();
            TinyMapper.Bind<ReservationDashboardReservationFutureDto, DbReservationDashboardReservationFuture>();
            TinyMapper.Bind<ReservationDashboardRoomsFutureDto, DbReservationDashboardRoomsFuture>();
            TinyMapper.Bind<ReservationDashboardRevenueFutureDto, DbReservationDashboardRevenueFuture>();
            TinyMapper.Bind<RatePlanStatisticFutureDto, DbRatePlanStatisticFuture>();
            TinyMapper.Bind<RatePlanStatisticFutureAuditDto, DbRatePlanStatisticFutureAudit>();
            TinyMapper.Bind<ReservationDashboardRoomsFutureAuditDto, DbReservationDashboardRoomsFutureAudit>();
            TinyMapper.Bind<ReservationDashboardOccupancyFutureAuditDto, DbReservationDashboardOccupancyFutureAudit>();
            TinyMapper.Bind<ReservationDashboardReservationFutureAuditDto, DbReservationDashboardReservationFutureAudit>();
            TinyMapper.Bind<ReservationDashboardRevenueFutureAuditDto, DbReservationDashboardRevenueFutureAudit>();
            TinyMapper.Bind<DailyHistoryDto, DbDailyHistory>();
            TinyMapper.Bind<GetFullReservationDetailsDto, DbGetFullReservationDetails>(); 
            TinyMapper.Bind<BanquetingRoomsDto, DbBanquetingRooms>(); 
            TinyMapper.Bind<SourcesDto, DbSources>(); 
            TinyMapper.Bind<SubSourcesDto, DbSubSources>(); 

            // Dto to Db List
            TinyMapper.Bind<List<SchedulerLogDto>, List<DbSchedulerLog>>();
            TinyMapper.Bind<List<SchedulerSetupDto>, List<DbSchedulerSetup>>();
            TinyMapper.Bind<List<ReservationDashboardRoomSummaryHistoryDto>, List<DbRoomSummaryHistory>>();
            TinyMapper.Bind<List<LedgerBalanceHistoryDto>, List<DbLedgerBalanceHistory>>();
            TinyMapper.Bind<List<RatePlanStatisticHistoryDto>, List<DbRatePlanStatisticHistory>>();
            TinyMapper.Bind<List<SourceStatisticHistoryDto>, List<DbSourceStatisticHistory>>();
            TinyMapper.Bind<List<ReservationDashboardOccupancyHistoryDto>, List<DbReservationDashboardOccupancyHistory>>();
            TinyMapper.Bind<List<ReservationDashboardReservationHistoryDto>, List<DbReservationDashboardReservationHistory>>();
            TinyMapper.Bind<List<ReservationDashboardRoomsHistoryDto>, List<DbReservationDashboardRoomsHistory>>();
            TinyMapper.Bind<List<ReservationDashboardRevenueHistoryDto>, List<DbReservationDashboardRevenueHistory>>();
            TinyMapper.Bind<List<SourceStatisticFutureDto>, List<DbSourceStatisticFuture>>();
            TinyMapper.Bind<List<SourceStatisticsFutureAuditDto>, List<DbSourceStatisticFutureAudit>>();
            TinyMapper.Bind<List<ReservationDashboardOccupancyFutureDto>, List<DbReservationDashboardOccupancyFuture>>();
            TinyMapper.Bind<List<ReservationDashboardReservationFutureDto>, List<DbReservationDashboardReservationFuture>>();
            TinyMapper.Bind<List<ReservationDashboardRoomsFutureDto>, List<DbReservationDashboardRoomsFuture>>();
            TinyMapper.Bind<List<ReservationDashboardRevenueFutureDto>, List<DbReservationDashboardRevenueFuture>>();
            TinyMapper.Bind<List<RatePlanStatisticFutureDto>, List<DbRatePlanStatisticFuture>>();
            TinyMapper.Bind<List<RatePlanStatisticFutureAuditDto>, List<DbRatePlanStatisticFutureAudit>>();
            TinyMapper.Bind<List<ReservationDashboardRoomsFutureAuditDto>, List<DbReservationDashboardRoomsFutureAudit>>();
            TinyMapper.Bind<List<ReservationDashboardOccupancyFutureAuditDto>, List<DbReservationDashboardOccupancyFutureAudit>>();
            TinyMapper.Bind<List<ReservationDashboardReservationFutureAuditDto>, List<DbReservationDashboardReservationFutureAudit>>();
            TinyMapper.Bind<List<ReservationDashboardRevenueFutureAuditDto>, List<DbReservationDashboardRevenueFutureAudit>>();
            TinyMapper.Bind<List<DailyHistoryDto>, List<DbDailyHistory>>();
            TinyMapper.Bind<List<GetFullReservationDetailsDto>, List<DbGetFullReservationDetails>>();
            TinyMapper.Bind<List<BanquetingRoomsDto>, List<DbBanquetingRooms>>();
            TinyMapper.Bind<List<SourcesDto>, List<DbSources>>();
            TinyMapper.Bind<List<SubSourcesDto>, List<DbSubSources>>();
           
        }
        public IDapperRepository<DbSchedulerSetup> SchedulerSetup => _schedulerSetup ??= new DapperRepository<DbSchedulerSetup>(Connection);
        public IDapperRepository<DbSchedulerLog> SchedulerLog => _schedulerlog ??= new DapperRepository<DbSchedulerLog>(Connection);
        public IDapperRepository<DbRoomSummaryHistory> RoomSummary => _roomSummary ??= new DapperRepository<DbRoomSummaryHistory>(Connection);
        public IDapperRepository<DbLedgerBalanceHistory> LedgerBalance => _ledgerBalance ??= new DapperRepository<DbLedgerBalanceHistory>(Connection);
        public IDapperRepository<DbRatePlanStatisticHistory> RatePlanStatisticHistory => _ratePlanStatistic ??= new DapperRepository<DbRatePlanStatisticHistory>(Connection);
        public IDapperRepository<DbRatePlanStatisticFuture> RatePlanStatisticFuture => _ratePlanStatisticFuture ??= new DapperRepository<DbRatePlanStatisticFuture>(Connection);
        public IDapperRepository<DbSourceStatisticHistory> SourceStatistic => _sourceStatistic ??= new DapperRepository<DbSourceStatisticHistory>(Connection);
        public IDapperRepository<DbReservationDashboardOccupancyHistory> Occupancy => _occupancy ??= new DapperRepository<DbReservationDashboardOccupancyHistory>(Connection);
        public IDapperRepository<DbReservationDashboardReservationHistory> Reservation => _reservation ??= new DapperRepository<DbReservationDashboardReservationHistory>(Connection);
        public IDapperRepository<DbReservationDashboardRoomsHistory> Rooms => _rooms ??= new DapperRepository<DbReservationDashboardRoomsHistory>(Connection);
        public IDapperRepository<DbReservationDashboardRevenueHistory> Revenue => _revenue ??= new DapperRepository<DbReservationDashboardRevenueHistory>(Connection);
        public IDapperRepository<DbSourceStatisticFuture> SourceStatisticFuture => _sourceStatisticFuture ??= new DapperRepository<DbSourceStatisticFuture>(Connection);
        public IDapperRepository<DbSourceStatisticFutureAudit> SourceStatisticFutureAudit => _sourceStatisticFutureAudit ??= new DapperRepository<DbSourceStatisticFutureAudit>(Connection);
        public IDapperRepository<DbReservationDashboardRevenueFuture> RevenueFuture => _revenueFuture ??= new DapperRepository<DbReservationDashboardRevenueFuture>(Connection);
        public IDapperRepository<DbReservationDashboardRoomsFuture> RoomsFuture => _roomsFuture ??= new DapperRepository<DbReservationDashboardRoomsFuture>(Connection);
        public IDapperRepository<DbReservationDashboardReservationFuture> ReservationFuture => _reservationFuture ??= new DapperRepository<DbReservationDashboardReservationFuture>(Connection);
        public IDapperRepository<DbReservationDashboardOccupancyFuture> OccupancyFuture => _occupancyFuture ??= new DapperRepository<DbReservationDashboardOccupancyFuture>(Connection);
        public IDapperRepository<DbRatePlanStatisticFutureAudit> RatePlanStatisticFutureAudit => _ratePlanStatisticFutureAudit ??= new DapperRepository<DbRatePlanStatisticFutureAudit>(Connection);
        public IDapperRepository<DbReservationDashboardRoomsFutureAudit> RoomsFutureAudit => _roomsFutureAudit ??= new DapperRepository<DbReservationDashboardRoomsFutureAudit>(Connection);
        public IDapperRepository<DbReservationDashboardOccupancyFutureAudit> OccupancyFutureAudit => _occupancyFutureAudit ??= new DapperRepository<DbReservationDashboardOccupancyFutureAudit>(Connection);
        public IDapperRepository<DbReservationDashboardReservationFutureAudit> ReservationFutureAudit => _reservationFutureAudit ??= new DapperRepository<DbReservationDashboardReservationFutureAudit>(Connection);
        public IDapperRepository<DbReservationDashboardRevenueFutureAudit> RevenueFutureAudit => _revenueFutureAudit ??= new DapperRepository<DbReservationDashboardRevenueFutureAudit>(Connection);
        public IDapperRepository<DbDailyHistory> DailyHistory => _dailyHistory ??= new DapperRepository<DbDailyHistory>(Connection);
        public IDapperRepository<DbGetFullReservationDetails> GetFullReservationDetails => _getFullReservationDetails ??= new DapperRepository<DbGetFullReservationDetails>(Connection);
        public IDapperRepository<DbBanquetingRooms> BanquetingRooms => _banquetingRooms ??= new DapperRepository<DbBanquetingRooms>(Connection);
        public IDapperRepository<DbSources> Sources => _sources ??= new DapperRepository<DbSources>(Connection);
        public IDapperRepository<DbSubSources> SubSources => _subsources ??= new DapperRepository<DbSubSources>(Connection);
    }
}
