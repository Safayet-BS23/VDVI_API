using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsBIRoomsHistoryRepository
    {
        Task<ReservationDashboardRoomsHistoryDto> InsertAsync(ReservationDashboardRoomsHistoryDto dto);
        Task<IEnumerable<ReservationDashboardRoomsHistoryDto>> BulkInsertAsync(IEnumerable<ReservationDashboardRoomsHistoryDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<ReservationDashboardRoomsHistoryDto> dto);
        Task<ReservationDashboardRoomsHistoryDto> UpdateAsync(ReservationDashboardRoomsHistoryDto dto);
        Task<IEnumerable<ReservationDashboardRoomsHistoryDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<ReservationDashboardRoomsHistoryDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode);
        Task<bool> DeleteByDashboardDateAsync(DateTime DashboardDate);
    }
}
