using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.Services.Interfaces
{
    public interface IRecordsToSyncChangedService
    {
        Task<Result<PrometheusResponse>> InsertAsync(RecordsToSyncChangedDto dto);
        Task<Result<PrometheusResponse>> BulkInsertAsync(List<RecordsToSyncChangedDto> dtos);
        Task<Result<PrometheusResponse>> BulkInsertWithProcAsync(List<RecordsToSyncChangedDto> dtos);
        Task<Result<PrometheusResponse>> GetByPropertCodeAsync(string propertyCode);
        Task<Result<PrometheusResponse>> DeleteByPropertyCodeAsync(string propertyCode);
    }
}
