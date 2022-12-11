using System.Threading.Tasks;
using VDVI.Services.Rebus.Models;

namespace VDVI.Services.MediatR.Services.Afas
{
    public interface IAfasEventService
    {
        Task ExecuteEventAsync(AfasSchedulerEvent schedulerEvent); 
    }
}
