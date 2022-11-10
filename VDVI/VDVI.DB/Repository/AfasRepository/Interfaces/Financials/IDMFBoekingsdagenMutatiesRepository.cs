using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.Repository.AfasDtos;
using VDVI.Repository.Dtos.AfasDtos;
using VDVI.Repository.Dtos.AfasDtos.AfasCommonDtos;

namespace VDVI.AfasRepository.Interfaces
{
    public interface IDMFBoekingsdagenMutatiesRepository
    {
        Task<DMFBoekingsdagenMutatiesDto> InsertAsync(DMFBoekingsdagenMutatiesDto dto);
        Task<IEnumerable<DMFBoekingsdagenMutatiesDto>> BulkInsertAsync(IEnumerable<DMFBoekingsdagenMutatiesDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<DMFBoekingsdagenMutatiesDto> dto);
        Task<DMFBoekingsdagenMutatiesDto> UpdateAsync(DMFBoekingsdagenMutatiesDto dto); 
    }
}
