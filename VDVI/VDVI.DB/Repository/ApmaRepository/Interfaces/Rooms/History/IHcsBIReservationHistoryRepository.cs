using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsBIReservationHistoryRepository
    {
        Task<ReservationDashboardReservationHistoryDto> InsertAsync(ReservationDashboardReservationHistoryDto dto);
        Task<IEnumerable<ReservationDashboardReservationHistoryDto>> BulkInsertAsync(IEnumerable<ReservationDashboardReservationHistoryDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<ReservationDashboardReservationHistoryDto> dto);
        Task<ReservationDashboardReservationHistoryDto> UpdateAsync(ReservationDashboardReservationHistoryDto dto);
        Task<IEnumerable<ReservationDashboardReservationHistoryDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<ReservationDashboardReservationHistoryDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode);
        Task<bool> DeleteByDashboardDateAsync(DateTime DashboardDate);
    }
}
