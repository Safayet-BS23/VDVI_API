using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System.Threading.Tasks;

namespace VDVI.Services.Interfaces
{
    public interface IHcsListSubSourcesService
    {
         Task<Result<PrometheusResponse>> HcsListSubSourcesServiceAsync();
    }
}
