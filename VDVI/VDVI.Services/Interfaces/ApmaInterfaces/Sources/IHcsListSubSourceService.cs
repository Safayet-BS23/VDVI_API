using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Collections.Generic; 
using System.Threading.Tasks;
using VDVI.DB.Dtos; 

namespace VDVI.Services.Interfaces
{
    public interface IHcsListSubSourceService
    {
        Task<Result<PrometheusResponse>> InsertAsync(SubSourcesDto dto);
        Task<Result<PrometheusResponse>> BulkInsertAsync(List<SubSourcesDto> dtos);
        Task<Result<PrometheusResponse>> BulkInsertWithProcAsync(List<SubSourcesDto> dtos);
        Task<Result<PrometheusResponse>> GetByPropertCodeAsync(string propertyCode);
        Task<Result<PrometheusResponse>> DeleteByPropertyCodeAsync(string propertyCode);
    }
}
