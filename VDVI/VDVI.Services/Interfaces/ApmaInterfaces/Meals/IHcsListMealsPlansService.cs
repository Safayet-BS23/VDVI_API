using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.Services.Interfaces
{
    public interface IHcsListMealsPlansService
    {
        Task<Result<PrometheusResponse>> InsertAsync(MealPlansDto dto);
        Task<Result<PrometheusResponse>> BulkInsertAsync(List<MealPlansDto> dtos);
        Task<Result<PrometheusResponse>> BulkInsertWithProcAsync(List<MealPlansDto> dtos);
        Task<Result<PrometheusResponse>> GetByPropertCodeAsync(string propertyCode);
        Task<Result<PrometheusResponse>> DeleteByPropertyCodeAsync(string propertyCode);
    }
}
