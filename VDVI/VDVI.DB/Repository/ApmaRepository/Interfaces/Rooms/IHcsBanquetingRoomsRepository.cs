using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsBanquetingRoomsRepository
    {
        Task<BanquetingRoomsDto> InsertAsync(BanquetingRoomsDto dto);
        Task<IEnumerable<BanquetingRoomsDto>> BulkInsertAsync(IEnumerable<BanquetingRoomsDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<BanquetingRoomsDto> dto);
        Task<BanquetingRoomsDto> UpdateAsync(BanquetingRoomsDto dto);
        Task<IEnumerable<BanquetingRoomsDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<BanquetingRoomsDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode); 
    }
}
