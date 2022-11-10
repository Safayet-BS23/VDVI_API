using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks; 
using VDVI.Services.Interfaces;

namespace VDVI.Client.Controllers.ApmaControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HcsGetFullReservationDetailsController : ControllerBase
    {
        public readonly IHcsGetFullReservationDetailsService _hcsGetFullReservationDetailsService;

        public HcsGetFullReservationDetailsController(IHcsGetFullReservationDetailsService hcsGetFullReservationDetailsService)
        {
            _hcsGetFullReservationDetailsService = hcsGetFullReservationDetailsService;
        }
        [HttpPost("HcsGetFullReservationDetails")]
        public async Task<IActionResult> HcsGetFullReservationDetails()
        {
            var response = await _hcsGetFullReservationDetailsService.HcsGetFullReservationDetailsAsync();
            return Ok(response);
        }
    }
}
