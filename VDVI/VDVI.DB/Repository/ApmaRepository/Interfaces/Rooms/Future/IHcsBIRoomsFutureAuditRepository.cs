using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsBIRoomsFutureAuditRepository
    {
        Task<ReservationDashboardRoomsFutureAuditDto> InsertAsync(ReservationDashboardRoomsFutureAuditDto dto);
        Task<IEnumerable<ReservationDashboardRoomsFutureAuditDto>> BulkInsertAsync(IEnumerable<ReservationDashboardRoomsFutureAuditDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<ReservationDashboardRoomsFutureAuditDto> dto);
        Task<ReservationDashboardRoomsFutureAuditDto> UpdateAsync(ReservationDashboardRoomsFutureAuditDto dto);
        Task<IEnumerable<ReservationDashboardRoomsFutureAuditDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<ReservationDashboardRoomsFutureAuditDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode);
        Task<bool> DeleteByDashboardDateAsync(DateTime DashboardDate);
    }
}
