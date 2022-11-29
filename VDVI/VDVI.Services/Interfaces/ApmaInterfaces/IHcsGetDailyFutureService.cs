using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Threading.Tasks;

namespace VDVI.Services.Interfaces
{
    public interface IHcsGetDailyFutureService
    {
        Task<Result<PrometheusResponse>> HcsGetDailyHistoryFutureAsyc(DateTime lastExecutionDate, int dayDifference);
        Task<Result<PrometheusResponse>> GetListHcsDailyHistoryFutureAsync(DateTime StartDate, DateTime EndDate, string propertyCode, string pmsSegmentType);
    }
}
