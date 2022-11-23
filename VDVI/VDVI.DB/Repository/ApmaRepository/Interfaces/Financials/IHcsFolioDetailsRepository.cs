using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsFolioDetailsRepository
    {
        Task<FolioDetailDto> InsertAsync(FolioDetailDto dto);
        Task<IEnumerable<FolioDetailDto>> BulkInsertAsync(IEnumerable<FolioDetailDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<FolioDetailDto> dto);
        Task<FolioDetailDto> UpdateAsync(FolioDetailDto dto);
        Task<IEnumerable<FolioDetailDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<FolioDetailDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode);
    }
}
