using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsListSourcesRepository
    {
        Task<SourcesDto> InsertAsync(SourcesDto dto);
        Task<IEnumerable<SourcesDto>> BulkInsertAsync(IEnumerable<SourcesDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<SourcesDto> dto);
        Task<SourcesDto> UpdateAsync(SourcesDto dto);
        Task<IEnumerable<SourcesDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<SourcesDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode); 
    }
}
