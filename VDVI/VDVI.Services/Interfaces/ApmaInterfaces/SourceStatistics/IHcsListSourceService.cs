using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Collections.Generic; 
using System.Threading.Tasks;
using VDVI.DB.Dtos; 

namespace VDVI.Services.Interfaces
{
    public interface IHcsListSourceService
    {
        Task<Result<PrometheusResponse>> InsertAsync(SourcesDto dto);
        Task<Result<PrometheusResponse>> BulkInsertAsync(List<SourcesDto> dtos);
        Task<Result<PrometheusResponse>> BulkInsertWithProcAsync(List<SourcesDto> dtos);
        Task<Result<PrometheusResponse>> GetByPropertCodeAsync(string propertyCode);
        Task<Result<PrometheusResponse>> DeleteByPropertyCodeAsync(string propertyCode);
    }
}
