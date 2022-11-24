using System.Threading.Tasks;

namespace VDVI.Services.Interfaces.APMA
{
    public interface IApmaTaskSchedulerService
    {
        Task SummaryScheduler();
        Task ResetStatusAsync();
    }
}
