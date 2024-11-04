using Microsoft.AspNetCore.Mvc;
using ValueObjects.DAL;

namespace ValueObjects.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DevController : ControllerBase
    {
        private ValueObjectsDbContext context;

        public DevController(ValueObjectsDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> InitDb()
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            return NoContent();
        }
    }
}
