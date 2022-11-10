using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using VDVI.Services.Interfaces;

namespace VDVI.Client.Controllers.ApmaControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HcsBIRatePlanStatisticsFutureController : ControllerBase
    {
        public readonly IHcsBIRatePlanStatisticsFutureService _hcsBIRatePlanStatisticsFutureService;

        public HcsBIRatePlanStatisticsFutureController(IHcsBIRatePlanStatisticsFutureService hcsBIRatePlanStatisticsFutureService)
        {
            _hcsBIRatePlanStatisticsFutureService = hcsBIRatePlanStatisticsFutureService;
        }
        [HttpPost("HcsBIRatePlanStatisticsFuture")]
        public async Task<IActionResult> HcsBIRatePlanStatisticsFuture(DateTime startDate, int daydifference)
        { 
            var response = await _hcsBIRatePlanStatisticsFutureService.HcsBIRatePlanStatisticsRepositoryFutureAsyc(startDate, daydifference); 
            return Ok(response);
        }

    }
}
