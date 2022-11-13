using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsBIReservationFutureRepository
    {
        Task<ReservationDashboardReservationFutureDto> InsertAsync(ReservationDashboardReservationFutureDto dto);
        Task<IEnumerable<ReservationDashboardReservationFutureDto>> BulkInsertAsync(IEnumerable<ReservationDashboardReservationFutureDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<ReservationDashboardReservationFutureDto> dto);
        Task<ReservationDashboardReservationFutureDto> UpdateAsync(ReservationDashboardReservationFutureDto dto);
        Task<IEnumerable<ReservationDashboardReservationFutureDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<ReservationDashboardReservationFutureDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode);
        Task<bool> DeleteByDashboardDateAsync(DateTime DashboardDate);
    }
}
