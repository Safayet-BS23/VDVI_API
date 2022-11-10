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
    public class HcsGetDailyFutureController : ControllerBase
    {
        public readonly IHcsGetDailyFutureService _hcsGetDailyFutureService;

        public HcsGetDailyFutureController(IHcsGetDailyFutureService hcsGetDailyFutureService)
        {
            _hcsGetDailyFutureService = hcsGetDailyFutureService;
        }
        [HttpPost("HcsGetDailyFuture")]
        public async Task<IActionResult> HcsGetDailyFuture(DateTime startDate, int daydifference)
        {
            var response = await _hcsGetDailyFutureService.HcsGetDailyHistoryFutureAsyc(startDate, daydifference);
            return Ok(response);
        }

    }
}
