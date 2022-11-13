using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsBIRoomsFutureRepository
    {
        Task<ReservationDashboardRoomsFutureDto> InsertAsync(ReservationDashboardRoomsFutureDto dto);
        Task<IEnumerable<ReservationDashboardRoomsFutureDto>> BulkInsertAsync(IEnumerable<ReservationDashboardRoomsFutureDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<ReservationDashboardRoomsFutureDto> dto);
        Task<ReservationDashboardRoomsFutureDto> UpdateAsync(ReservationDashboardRoomsFutureDto dto);
        Task<IEnumerable<ReservationDashboardRoomsFutureDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<ReservationDashboardRoomsFutureDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode);
        Task<bool> DeleteByDashboardDateAsync(DateTime DashboardDate);
    }
}
