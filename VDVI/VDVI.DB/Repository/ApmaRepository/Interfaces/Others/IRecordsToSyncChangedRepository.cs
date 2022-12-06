using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IRecordsToSyncChangedRepository
    {
        Task<RecordsToSyncChangedDto> InsertAsync(RecordsToSyncChangedDto dto);
        Task<IEnumerable<RecordsToSyncChangedDto>> BulkInsertAsync(IEnumerable<RecordsToSyncChangedDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<RecordsToSyncChangedDto> dto);
        Task<RecordsToSyncChangedDto> UpdateAsync(RecordsToSyncChangedDto dto);
        Task<IEnumerable<RecordsToSyncChangedDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<RecordsToSyncChangedDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode);
    }
}
