using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsListRateTypeRepository
    {
        Task<RateTypesDto> InsertAsync(RateTypesDto dto);
        Task<IEnumerable<RateTypesDto>> BulkInsertAsync(IEnumerable<RateTypesDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<RateTypesDto> dto);
        Task<RateTypesDto> UpdateAsync(RateTypesDto dto);
        Task<IEnumerable<RateTypesDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<RateTypesDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode); 
    }
}
