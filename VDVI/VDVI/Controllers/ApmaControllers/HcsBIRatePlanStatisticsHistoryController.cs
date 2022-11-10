using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using VDVI.Services.Interfaces;

namespace VDVI.Client.Controllers.ApmaControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HcsBIRatePlanStatisticsHistoryController : ControllerBase
    {
        public readonly IHcsBIRatePlanStatisticsHistoryService _hcsBIRatePlanStatisticsHistoryService;
        public HcsBIRatePlanStatisticsHistoryController(IHcsBIRatePlanStatisticsHistoryService hcsBIRatePlanStatisticsHistoryService)
        {
            _hcsBIRatePlanStatisticsHistoryService = hcsBIRatePlanStatisticsHistoryService;
        }

        [HttpPost("HcsBIRatePlanStatisticsHistory")]
        public async Task<IActionResult> HcsBIRatePlanStatisticsHistory(DateTime startDate, DateTime endDate)
        {
            var response = await _hcsBIRatePlanStatisticsHistoryService.HcsBIRatePlanStatisticsRepositoryHistoryAsyc(startDate, endDate);
            return Ok(response);    
        }

    }
}
