using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Threading.Tasks;

namespace VDVI.Services.Interfaces
{
    public interface IHcsGetDailyHistoryHistoryService
    {
        Task<Result<PrometheusResponse>> HcsGetDailyHistoryHistoryAsyc(DateTime StartDate, DateTime EndDate);
        Task<Result<PrometheusResponse>> GetListHcsDailyHistoryAsync(DateTime StartDate, DateTime EndDate, string propertyCode);
    }
}
