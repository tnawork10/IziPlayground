using Microsoft.AspNetCore.Mvc;

namespace SwaggerExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DevController : ControllerBase
    {
        [HttpGet("SyncContext")]
        public ActionResult SyncContext()
        {
            // контекста нет
            var sContext = SynchronizationContext.Current;
            // response будет IS NULL
            return Ok($"{sContext?.GetType().AssemblyQualifiedName ?? @"IS NULL"};");
        }
    }
}
