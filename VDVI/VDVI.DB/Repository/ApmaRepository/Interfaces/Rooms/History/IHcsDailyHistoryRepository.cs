using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsDailyHistoryRepository
    {
        Task<DailyHistoryDto> InsertAsync(DailyHistoryDto dto);
        Task<IEnumerable<DailyHistoryDto>> BulkInsertAsync(IEnumerable<DailyHistoryDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<DailyHistoryDto> dto);
        Task<DailyHistoryDto> UpdateAsync(DailyHistoryDto dto);
        Task<IEnumerable<DailyHistoryDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<DailyHistoryDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode);
        Task<bool> DeleteByDashboardDateAsync(DateTime DashboardDate);
    }
}
