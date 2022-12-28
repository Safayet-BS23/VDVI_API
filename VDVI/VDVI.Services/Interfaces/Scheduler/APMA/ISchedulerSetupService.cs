using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;
using VDVI.Services.MediatR.Models;

namespace VDVI.Services.Interfaces.APMA
{
    public interface ISchedulerSetupService
    {
        Task<Result<PrometheusResponse>> InsertAsync(SchedulerSetupDto dto);
        Task<Result<PrometheusResponse>> UpdateAsync(SchedulerSetupDto dto);
        Task<Result<PrometheusResponse>> SaveWithProcAsync(ApmaSchedulerEvent dto);
        Task<IEnumerable<SchedulerSetupDto>> FindByAllScheduleAsync();
        Task<SchedulerSetupDto> FindByMethodNameAsync(string methodName);
        Task<Result<PrometheusResponse>> FindByIdAsync(string schedulerName);
        Task<Result<PrometheusResponse>> ResetStatusAsync();
        Task<bool> DeleteByPropertyCodeAsync(string schedulerName);
    }
}
