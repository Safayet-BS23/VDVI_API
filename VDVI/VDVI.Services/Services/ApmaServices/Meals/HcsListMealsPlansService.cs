using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Exceptions;
using Framework.Core.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.ApmaRepository;
using VDVI.DB.Dtos;
using VDVI.Services.Interfaces;

namespace VDVI.Services
{
    public class HcsListMealsPlansService : IHcsListMealsPlansService
    {
        private readonly IMasterRepository _managementRepository;

        public HcsListMealsPlansService(IMasterRepository managementRepository)
        {
            _managementRepository = managementRepository;
        }

        public async Task<Result<PrometheusResponse>> InsertAsync(MealPlansDto dto)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    dto = await _managementRepository.HcsListMealPlansRepository.InsertAsync(dto);

                    return PrometheusResponse.Success(dto, "Data saved successful");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                    RethrowException = false
                });
        }

        public async Task<Result<PrometheusResponse>> BulkInsertAsync(List<MealPlansDto> dtos)
        {

            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    var resp = await _managementRepository.HcsListMealPlansRepository.BulkInsertAsync(dtos);

                    return PrometheusResponse.Success(resp, "Data saved successful");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                    RethrowException = false
                });
        }

        public async Task<Result<PrometheusResponse>> BulkInsertWithProcAsync(List<MealPlansDto> dtos)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    var resp = await _managementRepository.HcsListMealPlansRepository.BulkInsertWithProcAsync(dtos);

                    return PrometheusResponse.Success(resp, "Data saved successful");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                    RethrowException = false
                });
        }

        public async Task<Result<PrometheusResponse>> GetByPropertCodeAsync(string propertyCode)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    var dtos = await _managementRepository.HcsRoomSummaryHistoryRepository.GetAllByPropertyCodeAsync(propertyCode);

                    return PrometheusResponse.Success(dtos, "Data saved successful");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                    RethrowException = false
                });
        }

        public async Task<Result<PrometheusResponse>> DeleteByPropertyCodeAsync(string propertyCode)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    var dbroomSummariesRes = await _managementRepository.HcsRoomSummaryHistoryRepository.DeleteByPropertyCodeAsync(propertyCode);

                    return PrometheusResponse.Success("", "Data removal is successful");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                    RethrowException = false
                });
        }

       
    }
}
