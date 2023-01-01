using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Threading.Tasks;

namespace VDVI.Services.Interfaces
{
    public interface IHcsGetFullReservationDetailsService
    {
        Task<Result<PrometheusResponse>> HcsGetFullReservationDetailsAsync(DateTime BusinessStartDate);
    }
}
