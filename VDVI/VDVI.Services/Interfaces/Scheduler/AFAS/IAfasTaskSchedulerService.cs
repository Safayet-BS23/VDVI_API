using System.Threading.Tasks;

namespace VDVI.Services.Interfaces.Scheduler.AFAS
{
    public interface IAfasTaskSchedulerService
    {
        Task SummaryScheduler();

        Task ResetStatusAsync();
    }
}
