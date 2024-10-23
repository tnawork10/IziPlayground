using System.Collections.Generic;
using IziHardGames.Playgrounds.ForEfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EfCoreQuery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EFLeftJoinController : ControllerBase
    {
        private QueryDbContext context;

        public EFLeftJoinController(QueryDbContext context)
        {
            this.context = context;
        }

        [HttpPost(nameof(GetLeftJoin10To20))]
        public async Task<IActionResult> GetLeftJoin10To20()
        {
            var list = new List<int>();
            for (int i = 10; i < 20; i++)
            {
                list.Add(i);
            }
            var q = context.Ones.Join(list, x => x.Id, y => y, (x, _) => x);

            return Ok(await q.ToArrayAsync());
        }

        [HttpPost(nameof(GetLeftJoin4))]
        public async Task<IActionResult> GetLeftJoin4()
        {
            var q = context.EntityWithValues.Join([2], x => x.Repeat4, y => y, (x, _) => x);
            return Ok(await q.ToArrayAsync());
        }
    }

}
