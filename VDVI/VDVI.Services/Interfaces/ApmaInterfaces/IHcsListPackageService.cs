using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System.Threading.Tasks;

namespace VDVI.Services.Interfaces
{
    public interface IHcsListPackageService
    { 
         Task<Result<PrometheusResponse>> HcsListPackagesServiceeAsync();
    }
}
