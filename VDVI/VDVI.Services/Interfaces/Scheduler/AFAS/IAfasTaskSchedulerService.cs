using System.Threading.Tasks;

namespace VDVI.Services.Interfaces.AFAS
{
    public interface IAfasTaskSchedulerService
    {
        Task SummaryScheduler();

        Task ResetStatusAsync();
    }
}
