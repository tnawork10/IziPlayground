using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IziHardGames.Domain2;
using IziHardGames.Domain1;

namespace IziHardGames.MultiDomainApp.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.CompletedTask;
            var c1 = Class1.Run();
            var c2 = Class2.Run();
            return Ok(new { C1 = c1, C2 = c2 });
        }
    }
}
