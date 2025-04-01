using IziHardGames.Playgrounds.ForEfCore.Scenarios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using F = System.IO.File;
namespace MigratingWithRawSql.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DebugController(ILogger<DebugController> logger, MigratingWithRawSqlDbContext db) : ControllerBase
    {
        public const string FILE_SCRIPT01 = @"sql\insert_migrating.sql";
        public const string FILE_SCRIPT_QUERY_01 = @"sql\query_01.sql";

        [HttpGet(nameof(RecreateDb))]
        public async Task<IActionResult> RecreateDb()
        {
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();
            var changes = await db.InitAsync();
            return Ok(changes);
        }

        [HttpGet(nameof(ExecuteRaw01))]
        public async Task<IActionResult> ExecuteRaw01()
        {
            var sql = await F.ReadAllTextAsync(FILE_SCRIPT01);
            var result = await db.Database.ExecuteSqlRawAsync(sql);
            return Ok(await db.Entities01.ToArrayAsync());
        }

        [HttpGet(nameof(ExecuteRawQuery01))]
        public async Task<IActionResult> ExecuteRawQuery01()
        {
            var sql = await F.ReadAllTextAsync(FILE_SCRIPT_QUERY_01);
            var result = await db.Entities01.FromSqlRaw(sql).ToArrayAsync();
            return Ok(result);
        }
    }
}
