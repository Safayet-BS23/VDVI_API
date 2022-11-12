using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsListRateTypeRepository
    {
        Task<RateTypeDto> InsertAsync(RateTypeDto dto);
        Task<IEnumerable<RateTypeDto>> BulkInsertAsync(IEnumerable<RateTypeDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<RateTypeDto> dto);
        Task<RateTypeDto> UpdateAsync(RateTypeDto dto);
        Task<IEnumerable<RateTypeDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<RateTypeDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode); 
    }
}
