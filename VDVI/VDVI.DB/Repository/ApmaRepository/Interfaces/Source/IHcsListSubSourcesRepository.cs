using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsListSubSourcesRepository
    {
        Task<SubSourcesDto> InsertAsync(SubSourcesDto dto);
        Task<IEnumerable<SubSourcesDto>> BulkInsertAsync(IEnumerable<SubSourcesDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<SubSourcesDto> dto);
        Task<SubSourcesDto> UpdateAsync(SubSourcesDto dto);
        Task<IEnumerable<SubSourcesDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<SubSourcesDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode); 
    }
}
