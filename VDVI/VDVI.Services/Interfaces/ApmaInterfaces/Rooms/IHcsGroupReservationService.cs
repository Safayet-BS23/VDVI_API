using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.Services.Interfaces
{
    public interface IHcsGroupReservationService
    {
        Task<Result<PrometheusResponse>> InsertAsync(GroupReservationDto dto);
        Task<Result<PrometheusResponse>> BulkInsertAsync(List<GroupReservationDto> dtos);
        Task<Result<PrometheusResponse>> BulkInsertWithProcAsync(List<GroupReservationDto> dtos);
        Task<Result<PrometheusResponse>> GetByPropertCodeAsync(string propertyCode);
        Task<Result<PrometheusResponse>> DeleteByPropertyCodeAsync(string propertyCode);
    }
}
