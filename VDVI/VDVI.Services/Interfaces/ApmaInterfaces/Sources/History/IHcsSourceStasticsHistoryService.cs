using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.DB.Dtos;
using VDVI.DB.Dtos;

namespace VDVI.Services.Interfaces
{
    public interface IHcsSourceStasticsHistoryService
    {
        Task<Result<PrometheusResponse>> InsertAsync(SourceStatisticHistoryDto dto);
        Task<Result<PrometheusResponse>> BulkInsertAsync(List<SourceStatisticHistoryDto> dtos);
        Task<Result<PrometheusResponse>> BulkInsertWithProcAsync(List<SourceStatisticHistoryDto> dtos);
        Task<Result<PrometheusResponse>> GetByPropertCodeAsync(string propertyCode);
        Task<Result<PrometheusResponse>> DeleteByPropertyCodeAsync(string propertyCode);
        Task<Result<PrometheusResponse>> DeleteByBusinessDateAsync(DateTime businessDate);
    }
}
