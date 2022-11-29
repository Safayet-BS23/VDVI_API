using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Threading.Tasks;

namespace VDVI.Services.Interfaces
{
    public interface IHcsListGroupReservationService
    {
        Task<Result<PrometheusResponse>> HcsGetGroupReservationsAsync(DateTime startDate, DateTime endDate);
    }
}
