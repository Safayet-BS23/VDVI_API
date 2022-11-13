using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsBIOccupancyFutureAuditRepository
    {
        Task<ReservationDashboardOccupancyFutureAuditDto> InsertAsync(ReservationDashboardOccupancyFutureAuditDto dto);
        Task<IEnumerable<ReservationDashboardOccupancyFutureAuditDto>> BulkInsertAsync(IEnumerable<ReservationDashboardOccupancyFutureAuditDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<ReservationDashboardOccupancyFutureAuditDto> dto);
        Task<ReservationDashboardOccupancyFutureAuditDto> UpdateAsync(ReservationDashboardOccupancyFutureAuditDto dto);
        Task<IEnumerable<ReservationDashboardOccupancyFutureAuditDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<ReservationDashboardOccupancyFutureAuditDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode);
        Task<bool> DeleteByDashboardDateAsync(DateTime DashboardDate);
    }
}
