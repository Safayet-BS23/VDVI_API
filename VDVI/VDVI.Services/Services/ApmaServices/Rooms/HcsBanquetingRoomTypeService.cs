﻿using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Exceptions;
using Framework.Core.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.ApmaRepository;
using VDVI.DB.Dtos;
using VDVI.Repository.ApmaRepository;
using VDVI.Services.Interfaces;

namespace VDVI.Services
{
    public class HcsBanquetingRoomTypeService : IHcsListBanquetingRoomTypeService
    {
        private readonly IMasterRepository _managementRepository;

        public HcsBanquetingRoomTypeService(IMasterRepository managementRepository)
        {
            _managementRepository = managementRepository;
        }

        public async Task<Result<PrometheusResponse>> InsertAsync(BanquetingRoomTypesDto dto)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    dto = await _managementRepository.HcsListBanquetingRoomTypesRepository.InsertAsync(dto);

                    return PrometheusResponse.Success(dto, "Data saved successful");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                    RethrowException = false
                });
        }

        public async Task<Result<PrometheusResponse>> BulkInsertAsync(List<BanquetingRoomTypesDto> dtos)
        {

            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    var resp = await _managementRepository.HcsListBanquetingRoomTypesRepository.BulkInsertAsync(dtos);

                    return PrometheusResponse.Success(resp, "Data saved successful");
                },
                exception => new TryCatchExtensionResult<Result<PrometheusResponse>>
                {
                    DefaultResult = PrometheusResponse.Failure($"Error message: {exception.Message}. Details: {ExceptionExtension.GetExceptionDetailMessage(exception)}"),
                    RethrowException = false
                });
        }

        public async Task<Result<PrometheusResponse>> BulkInsertWithProcAsync(List<BanquetingRoomTypesDto> dtos)
        {
            return await TryCatchExtension.ExecuteAndHandleErrorAsync(
                async () =>
                {
                    var resp = await _managementRepository.HcsListBanquetingRoomTypesRepository.BulkInsertWithProcAsync(dtos);

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
                    var dtos = await _managementRepository.HcsListBanquetingRoomTypesRepository.GetAllByPropertyCodeAsync(propertyCode);

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
                    var dbroomSummariesRes = await _managementRepository.HcsListBanquetingRoomTypesRepository.DeleteByPropertyCodeAsync(propertyCode);

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
