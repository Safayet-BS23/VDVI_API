using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsRoomsRepository
    {
        Task<RoomsDto> InsertAsync(RoomsDto dto);
        Task<IEnumerable<RoomsDto>> BulkInsertAsync(IEnumerable<RoomsDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<RoomsDto> dto);
        Task<RoomsDto> UpdateAsync(RoomsDto dto);
        Task<IEnumerable<RoomsDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<RoomsDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode); 
    }
}
