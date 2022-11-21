using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsListRoomTypesRepository
    {
        Task<RoomTypesDto> InsertAsync(RoomTypesDto dto);
        Task<IEnumerable<RoomTypesDto>> BulkInsertAsync(IEnumerable<RoomTypesDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<RoomTypesDto> dto);
        Task<RoomTypesDto> UpdateAsync(RoomTypesDto dto);
        Task<IEnumerable<RoomTypesDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<RoomTypesDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode); 
    }
}
