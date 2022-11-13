using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsGetFullReservationDetailsRepository
    {
        Task<GetFullReservationDetailsDto> InsertAsync(GetFullReservationDetailsDto dto);
        Task<IEnumerable<GetFullReservationDetailsDto>> BulkInsertAsync(IEnumerable<GetFullReservationDetailsDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<GetFullReservationDetailsDto> dto);
        Task<GetFullReservationDetailsDto> UpdateAsync(GetFullReservationDetailsDto dto);
        Task<IEnumerable<GetFullReservationDetailsDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<GetFullReservationDetailsDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode); 
    }
}
