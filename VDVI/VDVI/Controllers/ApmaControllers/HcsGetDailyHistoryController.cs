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
    public class HcsGetDailyHistoryController : ControllerBase
    {
        public readonly IHcsGetDailyHistoryService _hcsGetDailyHistoryService;

        public HcsGetDailyHistoryController(IHcsGetDailyHistoryService hcsGetDailyHistoryService)
        {
            _hcsGetDailyHistoryService = hcsGetDailyHistoryService;
        }
        [HttpPost("HcsGetDailyHistory")]
        public async Task<IActionResult> HcsGetDailyHistory(DateTime startDate, DateTime endDate)
        {
            var response = await _hcsGetDailyHistoryService.HcsGetDailyHistoryAsyc(startDate, endDate);
            return Ok(response);
        }
    }
}
