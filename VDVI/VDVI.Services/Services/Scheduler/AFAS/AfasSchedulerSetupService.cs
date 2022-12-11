using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Exceptions;
using Framework.Core.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.AfasRepository; 
using VDVI.Repository.Models.AfasModels.Dto;
using VDVI.Services.Interfaces.AFAS;
using VDVI.Services.Services.BaseService;

namespace VDVI.Services.AFAS
{
    public class AfasSchedulerSetupService : AfasBaseService, IAfasSchedulerSetupService
    {
        private readonly IAfasMasterRepositroy _masterRepository;

        public AfasSchedulerSetupService(IAfasMasterRepositroy masterRepository)
        {
            _masterRepository = masterRepository;
        }
        public async Task<Result<PrometheusResponse>> InsertAsync(AfasSchedulerSetupDto dto)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    await _masterRepository.AfasSchedulerSetupRepository.InsertAsync(dto);
                    return PrometheusResponse.Success("", "Data Insert is successful");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {exception.GetExceptionDetailMessage()}"),
                    RethrowException = false
                });
        }
        public async Task<Result<PrometheusResponse>> UpdateAsync(AfasSchedulerSetupDto dto)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                 async () =>
                 {
                     var resp= await _masterRepository.AfasSchedulerSetupRepository.UpdateAsync(dto);
                     return PrometheusResponse.Success(resp, "Data Update is successful");
                 },
                 exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                 {
                     DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {exception.GetExceptionDetailMessage()}"),
                     RethrowException = false
                 });
        }
        public async Task<Result<PrometheusResponse>> SaveWithProcAsync(AfasSchedulerSetupDto dto)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    var resp = await _masterRepository.AfasSchedulerSetupRepository.SaveWithProcAsync(dto);

                    return PrometheusResponse.Success(resp, "Data saved successful");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {exception.GetExceptionDetailMessage()}"),
                    RethrowException = false
                });
        }
        public async Task<IEnumerable<AfasSchedulerSetupDto>> FindByAllScheduleAsync()
        {
            var result = await _masterRepository.AfasSchedulerSetupRepository.FindByAllScheduleAsync();

            return result;
        }

        public async Task<Result<PrometheusResponse>> ResetStatusAsync()
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    await _masterRepository.AfasSchedulerSetupRepository.ResetScheduleStatusAsync();
                    return PrometheusResponse.Success("", "AFAS Scheduler Status reset successfully.");
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
