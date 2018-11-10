using Microsoft.AspNetCore.Mvc;

namespace MicroService_Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        // GET
        public IActionResult Get()
        {
            return Ok("ok");
        }
    }
}