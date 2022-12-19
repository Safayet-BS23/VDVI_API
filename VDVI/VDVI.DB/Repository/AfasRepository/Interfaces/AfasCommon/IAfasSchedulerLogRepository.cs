using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.Repository.Models.AfasModels.Dto;

namespace VDVI.AfasRepository.Interfaces
{
    public interface IAfasSchedulerLogRepository
    {
        Task<AfasSchedulerLogDto> InsertAsync(AfasSchedulerLogDto dto);
        Task<Result<PrometheusResponse>> SaveWithProcAsync(string methodName,int logDayLimits, DateTime currentDateTime);
        Task<IEnumerable<AfasSchedulerLogDto>> BulkInsertAsync(IEnumerable<AfasSchedulerLogDto> dto);
        Task<AfasSchedulerLogDto> UpdateAsync(AfasSchedulerLogDto dto); 
        Task<AfasSchedulerLogDto> FindByMethodNameAsync(string methodName);
        Task<bool> DeleteByMethodNameAsync(string methodName);
        Task<bool> DeleteByDateAsync(DateTime executionDate);
        Task<IEnumerable<AfasSchedulerLogDto>> GetAllByMethodNameAsync(string methodName);
    }
}
