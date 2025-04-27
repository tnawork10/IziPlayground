using Microsoft.AspNetCore.Mvc;

namespace DevPlayground.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DevController(ILogger<DevController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> LogInfo()
        {
            logger.LogInformation($"This is information log {DateTime.Now}");
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> LogError()
        {
            var exc = new InvalidOperationException($"This is invalid exception {DateTime.Now}");
            var agr = new AggregateException(exc);
            logger.LogError(agr, agr.Message);
            return Ok();
        }
    }
}
