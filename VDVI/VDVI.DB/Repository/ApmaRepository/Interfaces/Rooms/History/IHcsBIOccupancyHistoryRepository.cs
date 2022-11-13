using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.DB.Dtos; 

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsBIOccupancyHistoryRepository
    {
        Task<ReservationDashboardOccupancyHistoryDto> InsertAsync(ReservationDashboardOccupancyHistoryDto dto);
        Task<IEnumerable<ReservationDashboardOccupancyHistoryDto>> BulkInsertAsync(IEnumerable<ReservationDashboardOccupancyHistoryDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<ReservationDashboardOccupancyHistoryDto> dto);
        Task<ReservationDashboardOccupancyHistoryDto> UpdateAsync(ReservationDashboardOccupancyHistoryDto dto);
        Task<IEnumerable<ReservationDashboardOccupancyHistoryDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<ReservationDashboardOccupancyHistoryDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode);
        Task<bool> DeleteByDashboardDateAsync(DateTime DashboardDate);
    }
}
