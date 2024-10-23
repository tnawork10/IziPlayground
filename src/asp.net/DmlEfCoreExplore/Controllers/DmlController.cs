using IziHardGames.Playgrounds.ForEfCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DmlEfCoreExplore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DmlController : ControllerBase
    {
        private DmlResearchContext context;
        public static readonly Guid guid0 = Guid.Parse("4ff17088-ae87-4103-959d-cfc29592a309");
        public static readonly Guid guid1 = Guid.Parse("47065d40-9427-4bff-91b5-d35b8bbe5b51");

        public DmlController(DmlResearchContext context)
        {
            this.context = context;
        }
        [HttpGet(nameof(EnsureDb))]
        public async Task<IActionResult> EnsureDb()
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            context.Entities.Add(new DmlEntity()
            {
                Id = guid0,
            });
            context.Entities.Add(new DmlEntity()
            {
                Id = guid1,
            });
            await context.SaveChangesAsync();
            return Ok(await context.Entities.ToArrayAsync());
        }

        [HttpPut(nameof(UpdateEntity))]
        public async Task<IActionResult> UpdateEntity()
        {
            context.Entities.Update(new DmlEntity()
            {
                Id = guid0,
                IntField = 1
            });
            await context.SaveChangesAsync();
            return Ok(await context.Entities.ToArrayAsync());
        }
    }
}
