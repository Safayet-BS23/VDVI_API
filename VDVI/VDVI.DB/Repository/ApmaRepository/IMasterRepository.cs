﻿using Framework.Core.Repository;
using VDVI.ApmaRepository.Interfaces;
using VDVI.Repository.ApmaRepository.Implementation; 

namespace VDVI.ApmaRepository
{
    public interface IMasterRepository : IProRepository
    {
        // Common
        ISchedulerSetupRepository SchedulerSetupRepository { get; }
        ISchedulerLogRepository SchedulerLogRepository { get; }

        // Accounts
        IHcsLedgerBalanceHistoryRepository HcsLedgerBalanceRepository { get; }
        IHcsBIRatePlanStatisticsHistoryRepository HcsBIRatePlanStatisticsHistoryRepository { get; }
        IHcsBIRatePlanStatisticsFutureRepository HcsBIRatePlanStatisticsFutureRepository { get; }
        IHcsBIRevenueHistoryRepository HcsBIRevenueHistoryRepository { get; }
        IHcsBIRevenueFutureRepository HcsBIRevenueFutureRepository { get; }

        // RoomSummary
        IHcsRoomSummaryHistoryRepository HcsRoomSummaryHistoryRepository { get; }
        IHcsBIOccupancyHistoryRepository HcsBIOccupancyHistoryRepository { get; }
        IHcsBIOccupancyFutureRepository HcsBIOccupancyFutureRepository { get; }
        IHcsBIRoomsHistoryRepository HcsBIRoomsHistoryRepository { get; }
        IHcsBIRoomsFutureRepository HcsBIRoomsFutureRepository { get; }
        IHcsBIReservationHistoryRepository HcsBIReservationHistoryRepository { get; }
        IHcsBIReservationFutureRepository HcsBIReservationFutureRepository { get; }
        IHcsDailyHistoryRepository HcsDailyHistoryRepository { get; }
        IHcsDailyHistoryFutureRepository HcsDailyHistoryFutureRepository { get; }
        IHcsDailyFutureAuditRepository HcsDailyHistoryFutureAuditRepository { get; }

        // SourceStatistics
        IHcsBISourceStatisticsHistoryRepository HcsBISourceStatisticsHistoryRepository { get; }
        IHcsBISourceStatisticsFutureRepository HcsBISourceStatisticsFutureRepository { get; }
        IHcsBISourceStatisticsFutureAuditRepository HcsBISourceStatisticsFutureAuditRepository { get; }

}
}
