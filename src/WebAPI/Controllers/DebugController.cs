using Microsoft.AspNetCore.Mvc;
using WebAPI.Json;

namespace IziPlayGround.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DebugController : ControllerBase
    {
        [HttpPost(nameof(DetectJsonConverterMiddleware))]
        public async Task<IActionResult> DetectJsonConverterMiddleware([FromBody] ValueObjectOfString? valueObjectOfString)
        {
            return Ok();
        }
    }
}
