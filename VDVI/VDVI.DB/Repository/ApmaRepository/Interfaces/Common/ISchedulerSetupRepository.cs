using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;
using VDVI.Repository.DB;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface ISchedulerSetupRepository
    {
        Task<SchedulerSetupDto> InsertAsync(SchedulerSetupDto dto); 
        Task<Result<PrometheusResponse>> SaveWithProcAsync(SchedulerSetupDto dto);
        Task<DbSchedulerSetup> FindByMethodNameAsync(string methodName);
        Task<SchedulerSetupDto> UpdateAsync(SchedulerSetupDto dto);
        Task<IEnumerable<SchedulerSetupDto>> FindByAllScheduleAsync();
        Task<SchedulerSetupDto> FindByIdAsync(string schedulerName);
        Task<bool> DeleteByPropertyCodeAsync(string schedulerName);
        Task ResetScheduleStatusAsync();
    }
}
