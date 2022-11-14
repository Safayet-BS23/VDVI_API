
using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.Services.Interfaces
{
    public interface IHcsRoomsService
    {
        Task<Result<PrometheusResponse>> InsertAsync(RoomsDto dto);
        Task<Result<PrometheusResponse>> BulkInsertAsync(List<RoomsDto> dtos);
        Task<Result<PrometheusResponse>> BulkInsertWithProcAsync(List<RoomsDto> dtos);
        Task<Result<PrometheusResponse>> GetByPropertCodeAsync(string propertyCode);
        Task<Result<PrometheusResponse>> DeleteByPropertyCodeAsync(string propertyCode);
        Task<Result<PrometheusResponse>> DeleteByBusinessDateAsync(DateTime businessDate);
    }
}
