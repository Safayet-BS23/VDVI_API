using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.Services.Interfaces
{
    public interface IHcsListRateTypeService
    {
        Task<Result<PrometheusResponse>> InsertAsync(RateTypeDto dto);
        Task<Result<PrometheusResponse>> BulkInsertAsync(List<RateTypeDto> dtos);
        Task<Result<PrometheusResponse>> BulkInsertWithProcAsync(List<RateTypeDto> dtos);
        Task<Result<PrometheusResponse>> GetByPropertCodeAsync(string propertyCode);
        Task<Result<PrometheusResponse>> DeleteByPropertyCodeAsync(string propertyCode); 
    }
}
