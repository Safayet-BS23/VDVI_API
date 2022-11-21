using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsListBanquetingRoomTypesRepository
    {
        Task<BanquetingRoomTypesDto> InsertAsync(BanquetingRoomTypesDto dto);
        Task<IEnumerable<BanquetingRoomTypesDto>> BulkInsertAsync(IEnumerable<BanquetingRoomTypesDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<BanquetingRoomTypesDto> dto);
        Task<BanquetingRoomTypesDto> UpdateAsync(BanquetingRoomTypesDto dto);
        Task<IEnumerable<BanquetingRoomTypesDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<BanquetingRoomTypesDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode); 
    }
}
