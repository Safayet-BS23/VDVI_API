
using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.Services.Interfaces
{
    public interface IHcsListBanquetingRoomTypeService
    {
        Task<Result<PrometheusResponse>> InsertAsync(BanquetingRoomTypesDto dto);
        Task<Result<PrometheusResponse>> BulkInsertAsync(List<BanquetingRoomTypesDto> dtos);
        Task<Result<PrometheusResponse>> BulkInsertWithProcAsync(List<BanquetingRoomTypesDto> dtos);
        Task<Result<PrometheusResponse>> GetByPropertCodeAsync(string propertyCode);
        Task<Result<PrometheusResponse>> DeleteByPropertyCodeAsync(string propertyCode); 
    }
}
