using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Threading.Tasks;

namespace VDVI.Services.Interfaces
{
    public interface IHcsListFolioDetailService
    {
        Task<Result<PrometheusResponse>> HcsListFolioDetailAsync(DateTime BusinesStartDate);
    }
}
