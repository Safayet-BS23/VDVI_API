using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsDailyHistoryFutureRepository
    {
        Task<DailyHistoryFutureDto> InsertAsync(DailyHistoryFutureDto dto);
        Task<IEnumerable<DailyHistoryFutureDto>> BulkInsertAsync(IEnumerable<DailyHistoryFutureDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<DailyHistoryFutureDto> dto);
        Task<DailyHistoryFutureDto> UpdateAsync(DailyHistoryFutureDto dto);
        Task<IEnumerable<DailyHistoryFutureDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<DailyHistoryFutureDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode);
        Task<bool> DeleteByDashboardDateAsync(DateTime DashboardDate);
    }
}
