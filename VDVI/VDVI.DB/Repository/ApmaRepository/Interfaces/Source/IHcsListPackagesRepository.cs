using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsListPackagesRepository
    {
        Task<PackagesDto> InsertAsync(PackagesDto dto);
        Task<IEnumerable<PackagesDto>> BulkInsertAsync(IEnumerable<PackagesDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<PackagesDto> dto);
        Task<PackagesDto> UpdateAsync(PackagesDto dto);
        Task<IEnumerable<PackagesDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<PackagesDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode); 
    }
}
