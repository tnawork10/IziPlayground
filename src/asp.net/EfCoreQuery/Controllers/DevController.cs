using IziHardGames.Playgrounds.ForEfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EfCoreQuery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevController : ControllerBase
    {
        private QueryDbContext context;

        public DevController(QueryDbContext context)
        {
            this.context = context;
        }

        [HttpPost(nameof(EnsureDbCreated))]
        public async Task<IActionResult> EnsureDbCreated()
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            await context.PopulateCompositeKeyJoins();
            await context.PopulateEntityPkSimples();
            return NoContent();
        }

        [HttpPost(nameof(PopulateDb))]
        public async Task<IActionResult> PopulateDb()
        {
            for (int i = 1; i < 20; i++)
            {
                var one = new EntQueryOne()
                {
                    Id = i,
                };
                for (int j = 0; j < 50; j++)
                {
                    one.One = new EntQueryToOne()
                    {
                        One = one
                    };
                }

                var list = new List<EntQueryToMany>();
                for (int j = 0; j < 50; j++)
                {
                    var many = new EntQueryToMany()
                    {
                        One = one,
                    };
                    list.Add(many);
                }
                one.Many = list;
                context.Ones.Add(one);
            }
            Populate(context.EntityWithValues);
            await context.SaveChangesAsync();
            return Ok(await context.Ones.Include(x => x.One).Include(x => x.Many).ToArrayAsync());
        }

        private static void Populate(DbSet<EntityWithValue> entityWithValues)
        {
            for (int i = 0; i < 100; i++)
            {
                var v1 = new EntityWithValue()
                {
                    Id = Guid.NewGuid(),
                    Repeat2 = i / 2 + i % 2,
                    Repeat4 = i / 4 + i % 4,
                };
                entityWithValues.Add(v1);
            }
        }
    }
}
