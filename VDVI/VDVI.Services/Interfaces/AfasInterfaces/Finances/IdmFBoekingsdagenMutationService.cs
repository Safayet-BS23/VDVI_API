using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.Repository.AfasDtos;
using VDVI.Repository.Dtos.AfasDtos;
using VDVI.Repository.Dtos.AfasDtos.AfasCommonDtos;

namespace VDVI.Services.AfasInterfaces
{
    public interface IdmFBoekingsdagenMutationService
    {
        Task<Result<PrometheusResponse>> InsertAsync(DMFBoekingsdagenMutatiesDto dto);
        Task<Result<PrometheusResponse>> BulkInsertAsync(List<DMFBoekingsdagenMutatiesDto> dtos);
        Task<Result<PrometheusResponse>> BulkInsertWithProcAsync(List<DMFBoekingsdagenMutatiesDto> dto);   
    }
}
