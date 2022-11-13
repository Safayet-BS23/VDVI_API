using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsBIOccupancyFutureRepository
    {
        Task<ReservationDashboardOccupancyFutureDto> InsertAsync(ReservationDashboardOccupancyFutureDto dto);
        Task<IEnumerable<ReservationDashboardOccupancyFutureDto>> BulkInsertAsync(IEnumerable<ReservationDashboardOccupancyFutureDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<ReservationDashboardOccupancyFutureDto> dto);
        Task<ReservationDashboardOccupancyFutureDto> UpdateAsync(ReservationDashboardOccupancyFutureDto dto);
        Task<IEnumerable<ReservationDashboardOccupancyFutureDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<ReservationDashboardOccupancyFutureDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode);
        Task<bool> DeleteByDashboardDateAsync(DateTime DashboardDate);
    }
}
