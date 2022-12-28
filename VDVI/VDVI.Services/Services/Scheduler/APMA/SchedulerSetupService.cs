using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Enums;
using Framework.Core.Exceptions;
using Framework.Core.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.ApmaRepository;
using VDVI.DB.Dtos;
using VDVI.Services.Interfaces.APMA;
using VDVI.Services.MediatR.Models;
using Serilog;

namespace VDVI.Services.APMA
{
    public class SchedulerSetupService : ApmaBaseService, ISchedulerSetupService
    {
        private readonly IMasterRepository _masterRepository;

        public SchedulerSetupService(IMasterRepository masterRepository)
        {
            _masterRepository = masterRepository;
        }
        public async Task<Result<PrometheusResponse>> InsertAsync(SchedulerSetupDto dto)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    await _masterRepository.SchedulerSetupRepository.InsertAsync(dto);
                    return PrometheusResponse.Success("", "Data Insert is successful");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {exception.GetExceptionDetailMessage()}"),
                    RethrowException = false
                });
        }
        public async Task<Result<PrometheusResponse>> UpdateAsync(SchedulerSetupDto dto)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                 async () =>
                 {
                     await _masterRepository.SchedulerSetupRepository.UpdateAsync(dto);
                     return PrometheusResponse.Success("", "Data Update is successful");
                 },
                 exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                 {
                     DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {exception.GetExceptionDetailMessage()}"),
                     RethrowException = false
                 });
        }
        public async Task<Result<PrometheusResponse>> SaveWithProcAsync(ApmaSchedulerEvent dto)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    DateTime? dateTime = null;
                    dto.Scheduler.LastExecutionDateTime = DateTime.UtcNow;
                    dto.Scheduler.NextExecutionDateTime = dto.Scheduler.NextExecutionDateTime.Value.AddMinutes(dto.Scheduler.ExecutionIntervalMins); //NextExecutionDateTime=NextExecutionDateTime+ExecutionIntervalMins
                    dto.Scheduler.LastBusinessDate = dto.Scheduler.isFuture == false ?  dto.EndDate?.Date : dateTime; //_Future does not need LastBusinessDate, because tartingpoint is always To

                    dto.Scheduler.SchedulerStatus = SchedulerStatus.Succeed.ToString();

                    var res = await FindByMethodNameAsync(dto.Scheduler.SchedulerName);
                    Log.Information($"Step-5=>>Apma: Apma Scheduler Log Save Before: " + dto.Scheduler.SchedulerName + " NextExTime:-" + res.NextExecutionDateTime + " Current UTC TIME:-" + DateTime.UtcNow);
                    if(res.NextExecutionDateTime != null && res.NextExecutionDateTime <= DateTime.UtcNow)
                    {
                        var resp = await _masterRepository.SchedulerSetupRepository.SaveWithProcAsync(dto.Scheduler);
                        Log.Information($"Step-6=>>Apma: Apma Scheduler Log Save Afer: " + dto.Scheduler.SchedulerName + " NextExTime:-" + res.NextExecutionDateTime + " Current UTC TIME:-" + DateTime.UtcNow);
                    }

                    return PrometheusResponse.Success("", "Data saved successful");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {exception.GetExceptionDetailMessage()}"),
                    RethrowException = false
                });
        }
        public async Task<IEnumerable<SchedulerSetupDto>> FindByAllScheduleAsync()
        {
            var result = await _masterRepository.SchedulerSetupRepository.FindByAllScheduleAsync();

            return result;
        }

        public async Task<Result<PrometheusResponse>> ResetStatusAsync()
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    await _masterRepository.SchedulerSetupRepository.ResetScheduleStatusAsync();
                    return PrometheusResponse.Success("", "APMA Scheduler Status reset successfully.");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {exception.GetExceptionDetailMessage()}"),
                    RethrowException = false
                });
        }


        public async Task<SchedulerSetupDto> FindByMethodNameAsync(string methodName)
        {
            var result = await _masterRepository.SchedulerSetupRepository.FindByIdAsync(methodName);
            return result;
        }
        public async Task<Result<PrometheusResponse>> FindByIdAsync(string schedulerName)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteByPropertyCodeAsync(string schedulerName)
        {
            throw new NotImplementedException();
        }

    }
}
