using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.Services.Interfaces
{
    public interface IHcsListGroupReservationService
    {
        Task<Result<PrometheusResponse>> HcsGetGroupReservationsAsync(DateTime startDate, DateTime endDate);
        Task<Result<PrometheusResponse>> HcsSyncGroupReservationAsync(List<RecordsToSyncChangedDto> changeRecords);
    }
}
