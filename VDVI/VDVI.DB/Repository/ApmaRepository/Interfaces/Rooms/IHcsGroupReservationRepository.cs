using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsGroupReservationRepository
    {
        Task<GroupReservationDto> InsertAsync(GroupReservationDto dto);
        Task<GroupReservationDto> UpsertAsync(GroupReservationDto dto);
        Task<IEnumerable<GroupReservationDto>> BulkInsertAsync(IEnumerable<GroupReservationDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<GroupReservationDto> dto);
        Task<GroupReservationDto> UpdateAsync(GroupReservationDto dto);
        Task<IEnumerable<GroupReservationDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<GroupReservationDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode);
    }
}
