using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.Services.Interfaces
{
    public interface IHcsListPackagesService
    {
        Task<Result<PrometheusResponse>> InsertAsync(PackagesDto dto);
        Task<Result<PrometheusResponse>> BulkInsertAsync(List<PackagesDto> dtos);
        Task<Result<PrometheusResponse>> BulkInsertWithProcAsync(List<PackagesDto> dtos);
        Task<Result<PrometheusResponse>> GetByPropertCodeAsync(string propertyCode);
        Task<Result<PrometheusResponse>> DeleteByPropertyCodeAsync(string propertyCode);
    }
}
