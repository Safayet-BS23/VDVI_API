using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface ISchedulerLogRepository
    {
        Task<SchedulerLogDto> InsertAsync(SchedulerLogDto dto);
        Task<Result<PrometheusResponse>> SaveWithProcAsync(string methodName, int logDayLimits, DateTime currentDateTime);
        Task<IEnumerable<SchedulerLogDto>> BulkInsertAsync(IEnumerable<SchedulerLogDto> dto);
        Task<SchedulerLogDto> UpdateAsync(SchedulerLogDto dto); 
        Task<SchedulerLogDto> FindByMethodNameAsync(string methodName);
        Task<bool> DeleteByMethodNameAsync(string methodName);
        Task<bool> DeleteByDateAsync(DateTime executionDate);
        Task<IEnumerable<SchedulerLogDto>> GetAllByMethodNameAsync(string methodName);
    }
}
