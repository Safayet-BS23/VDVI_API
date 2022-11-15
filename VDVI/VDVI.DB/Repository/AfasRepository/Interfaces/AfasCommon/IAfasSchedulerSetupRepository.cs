using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;
using VDVI.Repository.DB;
using VDVI.Repository.Models.AfasModels.Dto; 

namespace VDVI.AfasRepository.Interfaces
{
    public interface IAfasSchedulerSetupRepository
    {
        Task<AfasSchedulerSetupDto> InsertAsync(AfasSchedulerSetupDto dto); 
        Task<Result<PrometheusResponse>> SaveWithProcAsync(AfasSchedulerSetupDto dto);  
        Task<AfasSchedulerSetupDto> UpdateAsync(AfasSchedulerSetupDto dto, string schedulerName );
        Task<IEnumerable<AfasSchedulerSetupDto>> FindByAllScheduleAsync();
        Task<AfasSchedulerSetupDto> FindByIdAsync(string schedulerName);
        Task<bool> DeleteByPropertyCodeAsync(string schedulerName);
    }
}
