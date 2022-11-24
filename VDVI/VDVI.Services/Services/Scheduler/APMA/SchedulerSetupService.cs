using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Exceptions;
using Framework.Core.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.ApmaRepository;
using VDVI.DB.Dtos;
using VDVI.Services.Interfaces.APMA;

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
        public async Task<Result<PrometheusResponse>> SaveWithProcAsync(SchedulerSetupDto dto)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    var resp = await _masterRepository.SchedulerSetupRepository.SaveWithProcAsync(dto);

                    return PrometheusResponse.Success(resp, "Data saved successful");
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
                    return PrometheusResponse.Success("", "Scheduler Status reset successfully.");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {exception.GetExceptionDetailMessage()}"),
                    RethrowException = false
                });
        }


        public async Task<Result<PrometheusResponse>> FindByMethodNameAsync(string methodName)
        {
            throw new NotImplementedException();
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
