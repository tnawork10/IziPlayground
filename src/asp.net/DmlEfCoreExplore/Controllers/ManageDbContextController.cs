using DmlEfCoreExplore.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace DmlEfCoreExplore.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ManageDbContextController (PooledDbContext db): ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> RecreatePooledDbContext()
        {
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();
            return Ok();
        }
    }
}
