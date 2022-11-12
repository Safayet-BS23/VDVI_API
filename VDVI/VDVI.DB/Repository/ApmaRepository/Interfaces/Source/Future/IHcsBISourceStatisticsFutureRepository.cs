using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsBISourceStatisticsFutureRepository
    {
        Task<SourceStatisticFutureDto> InsertAsync(SourceStatisticFutureDto dto);
        Task<IEnumerable<SourceStatisticFutureDto>> BulkInsertAsync(IEnumerable<SourceStatisticFutureDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<SourceStatisticFutureDto> dto);
        Task<SourceStatisticFutureDto> UpdateAsync(SourceStatisticFutureDto dto);
        Task<IEnumerable<SourceStatisticFutureDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<SourceStatisticFutureDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode); 
        Task<bool> DeleteByBusinessDateAsync(DateTime businessDate);
    }
}
