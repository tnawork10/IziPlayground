using System;
using IziHardGames.Playgrounds.ForEfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EfCoreQuery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelectDateTimeController(TypesDbContext db) : ControllerBase
    {
        [HttpGet(nameof(RecreateDb))]
        public async Task<IActionResult> RecreateDb()
        {
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();
            return Ok();
        }

        [HttpGet()]
        public async Task<IActionResult> WhereDateOfDateTime()
        {
            var val = await db.DateTimes.Where(x => x.DateTime.Date == new DateTime(DateOnly.MaxValue, TimeOnly.MaxValue, DateTimeKind.Utc)).FirstOrDefaultAsync();
            return Ok(val);
        }
    }
}
