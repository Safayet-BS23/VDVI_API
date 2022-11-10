using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using VDVI.Services;
using VDVI.Services.Interfaces;

namespace VDVI.Client.Controllers.ApmaControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HcsBIReservationDashboardFutureController : ControllerBase
    {
        public IHcsBIReservationDashboardFutureService _hcsBIReservationDashboardFutureService;

        public HcsBIReservationDashboardFutureController(IHcsBIReservationDashboardFutureService hcsBIReservationDashboardFutureService)
        {
            _hcsBIReservationDashboardFutureService = hcsBIReservationDashboardFutureService;
        }
        [HttpPost("HcsBIReservationDashboardFuture")]
        public async Task<IActionResult> HcsBIReservationDashboardFuture(DateTime lastExecutionDate, int daydifference)
        {
            var response = await _hcsBIReservationDashboardFutureService.HcsBIReservationDashboardRepositoryAsyc(lastExecutionDate, daydifference);
            return Ok(response);
        }



    }
}
