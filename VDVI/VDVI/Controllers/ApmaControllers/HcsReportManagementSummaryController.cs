using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks; 
using VDVI.Services.Interfaces;
using static Framework.Constants.Constants;

namespace VDVI.Client.ApmaControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HcsReportManagementSummaryController : ControllerBase
    {
        private readonly IHcsReportManagementSummaryService _hcsBISourceStatisticsService;
        public HcsReportManagementSummaryController(IHcsReportManagementSummaryService hcsBISourceStatisticsService)
        {
            _hcsBISourceStatisticsService = hcsBISourceStatisticsService;
        }


        [HttpPost("HcsReportManagementSummary")]
        public async Task<IActionResult> HcsReportManagementSummary(DateTime startDate,DateTime endDate)
        {            
            var response = await _hcsBISourceStatisticsService.ReportManagementSummaryAsync(startDate, endDate);

            return Ok(response);
        }
    }
}
