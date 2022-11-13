
using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.Services.Interfaces
{
    public interface IHcsGetFullReservationDetailService
    {
        Task<Result<PrometheusResponse>> InsertAsync(GetFullReservationDetailsDto dto);
        Task<Result<PrometheusResponse>> BulkInsertAsync(List<GetFullReservationDetailsDto> dtos);
        Task<Result<PrometheusResponse>> BulkInsertWithProcAsync(List<GetFullReservationDetailsDto> dtos);
        Task<Result<PrometheusResponse>> GetByPropertCodeAsync(string propertyCode);
        Task<Result<PrometheusResponse>> DeleteByPropertyCodeAsync(string propertyCode); 
    }
}
