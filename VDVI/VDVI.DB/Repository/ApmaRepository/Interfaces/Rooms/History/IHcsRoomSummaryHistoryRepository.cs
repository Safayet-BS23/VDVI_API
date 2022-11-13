using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsRoomSummaryHistoryRepository
    {
        Task<ReservationDashboardRoomSummaryHistoryDto> InsertAsync(ReservationDashboardRoomSummaryHistoryDto dto);
        Task<IEnumerable<ReservationDashboardRoomSummaryHistoryDto>> BulkInsertAsync(IEnumerable<ReservationDashboardRoomSummaryHistoryDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<ReservationDashboardRoomSummaryHistoryDto> dto);
        Task<ReservationDashboardRoomSummaryHistoryDto> UpdateAsync(ReservationDashboardRoomSummaryHistoryDto dto);
        Task<IEnumerable<ReservationDashboardRoomSummaryHistoryDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<ReservationDashboardRoomSummaryHistoryDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode);
        Task<bool> DeleteByBusinessDateAsync(DateTime businessDate);
    }
}
