using Microsoft.AspNetCore.Mvc;

namespace TestWebFactory.B.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FactoryBController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Check()
        {
            return Ok("This is factory B");
        }
    }
}
