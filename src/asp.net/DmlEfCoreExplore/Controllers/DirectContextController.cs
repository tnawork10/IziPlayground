using DmlEfCoreExplore.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DmlEfCoreExplore.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DirectContextController(PooledDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetContextMeta()
        {
            var rec = await db.Entity01s.ToArrayAsync();
            return Ok(new { IdDb = db.Id, GuidDb = db.Guid });
        }

        [HttpGet]
        public async Task<IActionResult> GetContextMetaWithDispose()
        {
            var rec = await db.Entity01s.ToArrayAsync();
            await db.DisposeAsync();
            return Ok(new { IdDb = db.Id, GuidDb = db.Guid });
        }

        [HttpGet]
        public async Task<IActionResult> DisposedException()
        {
            await db.DisposeAsync();    // не вызывает ошибки?!
            var rec = await db.Entity01s.ToArrayAsync();
            return Ok(new { IdDb = db.Id, GuidDb = db.Guid, Data = rec });
        }
    }
}
