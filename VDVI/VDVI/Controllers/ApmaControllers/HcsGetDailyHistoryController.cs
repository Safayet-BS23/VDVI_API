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
        public readonly IHcsGetDailyHistoryHistoryService _hcsGetDailyHistoryService;

        public HcsGetDailyHistoryController(IHcsGetDailyHistoryHistoryService hcsGetDailyHistoryService)
        {
            _hcsGetDailyHistoryService = hcsGetDailyHistoryService;
        }
        [HttpPost("HcsGetDailyHistoryHistory")]
        public async Task<IActionResult> HcsGetDailyHistoryHistory(DateTime BusinessStartDate)
        {
            var response = await _hcsGetDailyHistoryService.HcsGetDailyHistoryHistoryAsyc(BusinessStartDate);
            return Ok(response);
        }
    }
}
