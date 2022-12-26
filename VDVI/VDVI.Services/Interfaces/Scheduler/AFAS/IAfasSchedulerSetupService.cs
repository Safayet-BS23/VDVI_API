using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;
using VDVI.Repository.Models.AfasModels.Dto;

namespace VDVI.Services.Interfaces.AFAS
{
    public interface IAfasSchedulerSetupService
    {
        Task<Result<PrometheusResponse>> InsertAsync(AfasSchedulerSetupDto dto);
        Task<Result<PrometheusResponse>> UpdateAsync(AfasSchedulerSetupDto dto);
        Task<Result<PrometheusResponse>> SaveWithProcAsync(AfasSchedulerSetupDto dto);
        Task<IEnumerable<AfasSchedulerSetupDto>> FindByAllScheduleAsync();
        Task<AfasSchedulerSetupDto> FindByMethodNameAsync(string methodName);
        Task<Result<PrometheusResponse>> FindByIdAsync(string schedulerName);
        Task<Result<PrometheusResponse>> ResetStatusAsync();
        Task<bool> DeleteByPropertyCodeAsync(string schedulerName);
    }
}
