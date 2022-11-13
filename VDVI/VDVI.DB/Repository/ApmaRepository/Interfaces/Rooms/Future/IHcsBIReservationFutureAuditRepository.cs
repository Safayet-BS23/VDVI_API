using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsBIReservationFutureAuditRepository
    {
        Task<ReservationDashboardReservationFutureAuditDto> InsertAsync(ReservationDashboardReservationFutureAuditDto dto);
        Task<IEnumerable<ReservationDashboardReservationFutureAuditDto>> BulkInsertAsync(IEnumerable<ReservationDashboardReservationFutureAuditDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<ReservationDashboardReservationFutureAuditDto> dto);
        Task<ReservationDashboardReservationFutureAuditDto> UpdateAsync(ReservationDashboardReservationFutureAuditDto dto);
        Task<IEnumerable<ReservationDashboardReservationFutureAuditDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<ReservationDashboardReservationFutureAuditDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode);
        Task<bool> DeleteByDashboardDateAsync(DateTime DashboardDate);
    }
}
