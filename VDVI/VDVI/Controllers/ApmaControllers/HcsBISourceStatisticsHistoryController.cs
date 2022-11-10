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
    public class HcsBISourceStatisticsHistoryController : ControllerBase
    {
        public IHcsBISourceStatisticsHistoryService _hcsBISourceStatisticsHistoryService;

        public HcsBISourceStatisticsHistoryController(IHcsBISourceStatisticsHistoryService hcsBISourceStatisticsHistoryService)
        {
            _hcsBISourceStatisticsHistoryService = hcsBISourceStatisticsHistoryService;
        }
        [HttpPost("HcsBISourceStatisticsHistory")]
        public async Task<IActionResult> HcsBISourceStatisticsHistory(DateTime startDate, DateTime endDate)
        {
            var response = await _hcsBISourceStatisticsHistoryService.HcsBIHcsBISourceStatisticsRepositoryHistoryAsyc(startDate, endDate);
            return Ok(response);
        }

    }
}
