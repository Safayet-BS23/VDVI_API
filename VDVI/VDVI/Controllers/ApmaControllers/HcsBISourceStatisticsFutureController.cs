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
    public class HcsBISourceStatisticsFutureController : ControllerBase
    {
        public readonly IHcsBISourceStatisticsFutureService _hcsBISourceStatisticsFutureService;
        public HcsBISourceStatisticsFutureController(IHcsBISourceStatisticsFutureService hcsBISourceStatisticsFutureService)
        {
            _hcsBISourceStatisticsFutureService = hcsBISourceStatisticsFutureService;
        }
        [HttpPost("HcsBISourceStatisticsFuture")]
        public async Task<IActionResult> HcsBIReservationDashboardHistory(DateTime startDate, int daydifference)
        {
            var response = await _hcsBISourceStatisticsFutureService.HcsBIHcsBISourceStatisticsRepositoryFutureAsyc(startDate, daydifference);
            return Ok(response);
        }
    }
}
