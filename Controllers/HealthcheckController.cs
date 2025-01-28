using Microsoft.AspNetCore.Mvc;

namespace SalesRecordCalculator.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class HealthcheckController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { status = "Healthy" });
        }
    }
}