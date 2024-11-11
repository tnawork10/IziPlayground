using IziHardGames.Playgrounds.ForEfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EfCoreQuery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompositeIndexScan(CompositeIndexScanDbContext context) : ControllerBase
    {
        [HttpPost(nameof(InitDb))]
        public async Task<IActionResult> InitDb()
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            context.Init();
            await context.SaveChangesAsync();
            return Ok(await context.EntityiesWithCompositeIndex.CountAsync());
        }

        [HttpPost(nameof(GetAny10))]
        public async Task<IActionResult> GetAny10()
        {
            return Ok(await context.EntityiesWithCompositeIndex.OrderBy(x => x.GuidFirst).Take(10).ToArrayAsync());
        }
    }
}
