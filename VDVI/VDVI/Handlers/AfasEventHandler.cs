using Rebus.Handlers;
using System.Threading.Tasks;
using VDVI.Services.Rebus.Models;
using VDVI.Services.MediatR.Services.Afas;

namespace VDVI.Client.Handlers
{
    public class AfasRebusEventHandler : IHandleMessages<AfasSchedulerEvent>
    {
        private readonly IAfasEventService afasEventService;
        public AfasRebusEventHandler(IAfasEventService afasEventService)
        {
            this.afasEventService = afasEventService;
        }


        public async Task Handle(AfasSchedulerEvent message)
        {
            await afasEventService.ExecuteEventAsync(message);
            await Task.CompletedTask;
        }
    }
}
