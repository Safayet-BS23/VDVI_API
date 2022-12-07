using System.Threading.Tasks;
using VDVI.Services.MediatR.Models;

namespace VDVI.Services.MediatR.Services.Apma
{
    public interface IApmaEventService
    {
        Task ExecuteEventAsync(ApmaSchedulerEvent schedulerEvent);
    }
}
