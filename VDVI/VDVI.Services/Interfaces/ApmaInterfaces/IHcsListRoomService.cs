using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System.Threading.Tasks;

namespace VDVI.Services.Interfaces
{
    public interface IHcsListRoomService
    {
         Task<Result<PrometheusResponse>> HcsListRoomsServiceAsync();
    }
}
