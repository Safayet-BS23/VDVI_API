using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsDailyFutureAuditRepository
    {
        Task<DailyHistoryFutureAuditDto> InsertAsync(DailyHistoryFutureAuditDto dto);
        Task<IEnumerable<DailyHistoryFutureAuditDto>> BulkInsertAsync(IEnumerable<DailyHistoryFutureAuditDto> dto); 
        Task<DailyHistoryFutureAuditDto> UpdateAsync(DailyHistoryFutureAuditDto dto);
        Task<IEnumerable<DailyHistoryFutureAuditDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<DailyHistoryFutureAuditDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode);
        Task<bool> DeleteByDashboardDateAsync(DateTime DashboardDate);
    }
}
