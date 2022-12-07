using Rebus.Handlers;
using System.Threading.Tasks;
using VDVI.Services.MediatR.Models;
using VDVI.Services.MediatR.Services.Apma;

namespace VDVI.Client.Handlers
{
    public class ApmaRebusEventHandler : IHandleMessages<ApmaSchedulerEvent>
    {
        private readonly IApmaEventService apmaEventService;
        public ApmaRebusEventHandler(IApmaEventService apmaEventService)
        {
            this.apmaEventService = apmaEventService;
        }


        public async Task Handle(ApmaSchedulerEvent message)
        {
            await apmaEventService.ExecuteEventAsync(message);
            await Task.CompletedTask;
        }
    }
}
