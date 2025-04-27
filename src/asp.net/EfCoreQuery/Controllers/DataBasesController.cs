using IziHardGames.Playgrounds.ForEfCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfCoreQuery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataBasesController(QueryDbContext queryDbContext, CompositeIndexScanDbContext compositeIndexScanDbContext) : ControllerBase
    {
        [HttpPost(nameof(CompositeIndexScanDbContextInit))]
        public async Task<IActionResult> CompositeIndexScanDbContextInit()
        {
            await compositeIndexScanDbContext.Database.EnsureDeletedAsync();
            await compositeIndexScanDbContext.Database.EnsureCreatedAsync();
            compositeIndexScanDbContext.Init();
            await compositeIndexScanDbContext.SaveChangesAsync();
            return Ok(await compositeIndexScanDbContext.EntityiesWithCompositeIndex.CountAsync());
        }

        [HttpPost(nameof(QueryDbContextInit))]
        public async Task<IActionResult> QueryDbContextInit()
        {
            var context = queryDbContext;
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            await context.Populate();
            await context.PopulateCompositeKeyJoins();
            await context.PopulateEntityPkSimples();
            await context.PopulateEntityWithCompositeUniqIndex();
            return NoContent();
        }
    }
}
