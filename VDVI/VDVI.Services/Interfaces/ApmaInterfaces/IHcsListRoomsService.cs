using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System.Threading.Tasks;

namespace VDVI.Services.Interfaces
{
    public interface IHcsListRoomsService
    {
         Task<Result<PrometheusResponse>> HcsListRoomsServiceAsync();
    }
}
