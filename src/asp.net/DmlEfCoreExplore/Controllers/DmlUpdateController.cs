using EFCore.BulkExtensions;
using IziHardGames.Playgrounds.ForEfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DmlEfCoreExplore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DmlUpdateController(DmlResearchContext context) : ControllerBase
    {
        [HttpGet(nameof(InitDatas))]
        public async Task<IActionResult> InitDatas()
        {
            int key = 1;
            for (int i = 1; i < 15; i++)
            {
                for (int j = 1; j < 15; j++)
                {
                    context.Add(new DmlUpdate()
                    {
                        Id = key++,
                        ValueAsDouble = double.MaxValue / i,
                        ValueAsInt = int.MinValue / j,
                        ValueAsString = $"{i}:{j}"
                    });
                }
            }
            await context.SaveChangesAsync();
            return Ok(await context.Updates.ToArrayAsync());
        }

        [HttpGet(nameof(UpdateBulk))]
        public async Task<IActionResult> UpdateBulk()
        {
            var list = new List<DmlUpdate>();
            for (int i = 1; i < 10; i++)
            {
                list.Add(new DmlUpdate()
                {
                    Id = i,
                    ValueAsDouble = 0,
                    ValueAsInt = 0,
                    ValueAsString = string.Empty
                    // all values must be valid even if they dont used in update
                });
            }
            await context.BulkUpdateAsync(list, x =>
             {
                 x.PropertiesToInclude = new List<string> { nameof(DmlUpdate.ValueAsInt), nameof(DmlUpdate.ValueAsString) };
             });
            return Ok(await context.Updates.OrderBy(x => x.Id).Take(15).ToArrayAsync());
        }
    }
}
