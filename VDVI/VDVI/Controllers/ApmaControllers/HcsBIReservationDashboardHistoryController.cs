using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using VDVI.Services.Interfaces;
using VDVI.Services.Services.ApmaServices;

namespace VDVI.Client.Controllers.ApmaControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HcsBIReservationDashboardHistoryController : ControllerBase
    {
        public IHcsBIReservationDashboardHistoryService _hcsBIReservationDashboardHistoryService;
        public HcsBIReservationDashboardHistoryController(IHcsBIReservationDashboardHistoryService hcsBIReservationDashboardHistoryService)
        {
            _hcsBIReservationDashboardHistoryService = hcsBIReservationDashboardHistoryService;
        }
        [HttpPost("HcsBIReservationDashboardHistory")]
        public async Task<IActionResult> HcsBIReservationDashboardHistory(DateTime startDate, DateTime endDate)
        {
            var response = await _hcsBIReservationDashboardHistoryService.HcsBIReservationDashboardRepositoryAsyc(startDate, endDate);
            return Ok(response);
        }

    }
}
