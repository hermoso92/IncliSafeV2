
using Microsoft.AspNetCore.Mvc;

namespace IncliSafe.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API funcionando correctamente");
        }
    }
}
