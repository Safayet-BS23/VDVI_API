using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VDVI.Services.MediatR.Models;
using VDVI.Services.MediatR.Services.Apma;

namespace VDVI.Client.Handlers
{
    public class ApmaEventHandler : IRequestHandler<ApmaSchedulerEvent>
    {
        private readonly IApmaEventService apmaEventService;
        public ApmaEventHandler(IApmaEventService apmaEventService)
        {
            this.apmaEventService = apmaEventService;
        }

        public async Task<Unit> Handle(ApmaSchedulerEvent request, CancellationToken cancellationToken)
        {
            await apmaEventService.ExecuteEventAsync(request);
            return Unit.Value;
        }
    }
}
