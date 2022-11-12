using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsBISourceStatisticsFutureAuditRepository
    {
        Task<SourceStatisticsFutureAuditDto> InsertAsync(SourceStatisticsFutureAuditDto dto);
        Task<IEnumerable<SourceStatisticsFutureAuditDto>> BulkInsertAsync(IEnumerable<SourceStatisticsFutureAuditDto> dto); 
        Task<SourceStatisticsFutureAuditDto> UpdateAsync(SourceStatisticsFutureAuditDto dto);
        Task<IEnumerable<SourceStatisticsFutureAuditDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<SourceStatisticsFutureAuditDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode); 
        Task<bool> DeleteByBusinessDateAsync(DateTime businessDate);
    }
}
