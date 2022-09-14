﻿using Framework.Core.Repository;
using MicroOrm.Dapper.Repositories;
using Microsoft.Extensions.Configuration;
using Nelibur.ObjectMapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks; 
using VDVI.DB.Dtos; 
using VDVI.DB.Models.Common;
using VDVI.Repository.DB;
using VDVI.Repository.Dtos.Accounts;
using VDVI.Repository.Dtos.ApmaDtos.Common;
using VDVI.Repository.Dtos.RoomSummary;
using VDVI.Repository.Dtos.SourceStatistics;

namespace VDVI.Repository.DbContext.ApmaDbContext
{

    public class VDVISchedulerDbContext : ProDbContext, IVDVISchedulerDbContext
    {
        private IDapperRepository<DbJobTaskScheduler> _taskScheduler;
        private IDapperRepository<DbSchedulerSetup> _schedulerSetup;
        private IDapperRepository<DbSchedulerLog> _schedulerlog;



        private IDapperRepository<DbRoomSummary> _roomSummary;
        private IDapperRepository<DbLedgerBalance> _ledgerBalance;

        private IDapperRepository<DbRatePlanStatisticHistory> _ratePlanStatistic;
        private IDapperRepository<DbSourceStatisticHistory> _sourceStatistic;
        private IDapperRepository<DbOccupancyHistory> _occupancy;
        private IDapperRepository<DbReservationHistory> _reservation;
        private IDapperRepository<DbRoomsHistory> _rooms;
        private IDapperRepository<DbRevenueHistory> _revenue;
        private IDapperRepository<DbSourceStatisticFuture> _sourceStatisticFuture;


        public VDVISchedulerDbContext(IConfiguration configuration) : base(new SqlConnection(configuration["ConnectionStrings:ApmaDb"]))
        {
            // Dto to Db - Single
            TinyMapper.Bind<JobTaskSchedulerDto, DbJobTaskScheduler>();
            TinyMapper.Bind<SchedulerSetupDto, DbSchedulerSetup>();
            TinyMapper.Bind<SchedulerSetupDto, DbSchedulerLog>();

            
            TinyMapper.Bind<RoomSummaryDto, DbRoomSummary>();
            TinyMapper.Bind<LedgerBalanceDto, DbLedgerBalance>();
            TinyMapper.Bind<RatePlanStatisticHistoryDto, DbRatePlanStatisticHistory>();
            TinyMapper.Bind<SourceStatisticHistoryDto, DbSourceStatisticHistory>();
            TinyMapper.Bind<OccupancyHistoryDto, DbOccupancyHistory>();
            TinyMapper.Bind<ReservationHistoryDto, DbReservationHistory>();
            TinyMapper.Bind<RoomsHistoryDto, DbRoomsHistory>();
            TinyMapper.Bind<RevenueHistoryDto, DbRevenueHistory>();
            TinyMapper.Bind<SourceStatisticFutureDto, DbSourceStatisticFuture>();

            // Dto to Db List
            TinyMapper.Bind<List<SchedulerSetupDto>,List<DbSchedulerSetup>>();
            TinyMapper.Bind<List<JobTaskSchedulerDto>, List<DbJobTaskScheduler>>();
            TinyMapper.Bind<List<RoomSummaryDto>, List<DbRoomSummary>>();
            TinyMapper.Bind<List<LedgerBalanceDto>, List<DbLedgerBalance>>();
            TinyMapper.Bind<List<RatePlanStatisticHistoryDto>, List<DbRatePlanStatisticHistory>>();
            TinyMapper.Bind<List<SourceStatisticHistoryDto>, List<DbSourceStatisticHistory>>();
            TinyMapper.Bind<List<OccupancyHistoryDto>, List<DbOccupancyHistory>>();
            TinyMapper.Bind<List<ReservationHistoryDto>, List<DbReservationHistory>>();
            TinyMapper.Bind<List<RoomsHistoryDto>, List<DbRoomsHistory>>();
            TinyMapper.Bind<List<RevenueHistoryDto>, List<DbRevenueHistory>>();
            TinyMapper.Bind<List<SourceStatisticFutureDto>, List<DbSourceStatisticFuture>>();
            TinyMapper.Bind<List<SchedulerSetupDto>,List<DbSchedulerLog>>();
        }
        public IDapperRepository<DbSchedulerSetup> SchedulerSetup => _schedulerSetup ??= new DapperRepository<DbSchedulerSetup>(Connection);
        public IDapperRepository<DbJobTaskScheduler> JobTaskScheduler => _taskScheduler ??= new DapperRepository<DbJobTaskScheduler>(Connection);
        public IDapperRepository<DbSchedulerLog> SchedulerLog => _schedulerlog ??= new DapperRepository<DbSchedulerLog>(Connection);
        public IDapperRepository<DbRoomSummary> RoomSummary => _roomSummary ??= new DapperRepository<DbRoomSummary>(Connection);
        public IDapperRepository<DbLedgerBalance> LedgerBalance => _ledgerBalance ??= new DapperRepository<DbLedgerBalance>(Connection);
        public IDapperRepository<DbRatePlanStatisticHistory> RatePlanStatisticHistory => _ratePlanStatistic ??= new DapperRepository<DbRatePlanStatisticHistory>(Connection);
        public IDapperRepository<DbSourceStatisticHistory> SourceStatistic => _sourceStatistic ??= new DapperRepository<DbSourceStatisticHistory>(Connection);
        public IDapperRepository<DbOccupancyHistory> Occupancy => _occupancy ??= new DapperRepository<DbOccupancyHistory>(Connection);
        public IDapperRepository<DbReservationHistory> Reservation => _reservation ??= new DapperRepository<DbReservationHistory>(Connection);
        public IDapperRepository<DbRoomsHistory> Rooms => _rooms ??= new DapperRepository<DbRoomsHistory>(Connection);
        public IDapperRepository<DbRevenueHistory> Revenue => _revenue ??= new DapperRepository<DbRevenueHistory>(Connection);
        public IDapperRepository<DbSourceStatisticFuture> SourceStatisticFuture => _sourceStatisticFuture ??= new DapperRepository<DbSourceStatisticFuture>(Connection);
    }
}