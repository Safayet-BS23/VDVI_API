
using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;
using VDVI.DB.Dtos;

namespace VDVI.Services.Interfaces
{
    public interface IHcsBIReservationHistoryService
    {
        Task<Result<PrometheusResponse>> InsertAsync(ReservationDashboardReservationHistoryDto dto);
        Task<Result<PrometheusResponse>> BulkInsertAsync(List<ReservationDashboardReservationHistoryDto> dtos);
        Task<Result<PrometheusResponse>> BulkInsertWithProcAsync(List<ReservationDashboardReservationHistoryDto> dtos);
        Task<Result<PrometheusResponse>> GetByPropertCodeAsync(string propertyCode);
        Task<Result<PrometheusResponse>> DeleteByPropertyCodeAsync(string propertyCode);
        Task<Result<PrometheusResponse>> DeleteByBusinessDateAsync(DateTime businessDate);
    }
}
