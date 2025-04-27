using IziHardGames.Playgrounds.ForEfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfCoreQuery.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConcurrentWriteController(IDbContextFactory<QueryDbContext> factory) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> ConcurrentWrite()
        {
            var tcs = new TaskCompletionSource();
            var tcs2 = new TaskCompletionSource();

            var t1 = Task.Run(async () =>
            {
                try
                {
                    var db1 = factory.CreateDbContext();
                    var es = await db1.EntityWithValues.OrderBy(x => x.Id).Take(10).ToArrayAsync();
                    tcs.SetResult();
                    foreach (var e in es)
                    {
                        e.Repeat2 = int.MinValue;
                        e.Repeat4 = int.MinValue;
                    }
                    await tcs2.Task;
                    await db1.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
                //await Task.Delay(TimeSpan.FromMinutes(10));
            });

            await tcs.Task;
            var db2 = factory.CreateDbContext();
            var es2 = await db2.EntityWithValues.OrderBy(x => x.Id).Take(10).ToArrayAsync();
            foreach (var e in es2)
            {
                e.Repeat2 = int.MaxValue;
                e.Repeat4 = int.MaxValue;
            }
            await db2.SaveChangesAsync();
            tcs2.SetResult();
            await t1;
            return Ok(await db2.EntityWithValues.OrderBy(x => x.Id).Take(10).ToArrayAsync());
        }
    }
}
