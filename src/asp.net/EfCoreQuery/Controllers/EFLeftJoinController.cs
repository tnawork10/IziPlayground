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

        [HttpPost(nameof(GetJoinLeftBoth))]
        public async Task<IActionResult> GetJoinLeftBoth()
        {
            var q = context.EntityWithValues.Join([0, 1, 1000], x => x.Repeat2, y => y, (x, y) => new { x, y });
            return Ok(await q.ToArrayAsync());
        }

        [HttpPost(nameof(GetJoinBoth))]
        public async Task<IActionResult> GetJoinBoth()
        {
            IEnumerable<int> es = [0, 1, 1000];
            var q = context.EntityWithValues.DefaultIfEmpty().Join(es, x => x.Repeat2, y => y, (x, y) => new { x, y }); // no

            var q2 = from ent in context.EntityWithValues
                     join e in es on ent.Repeat2 equals e into joined
                     from eOrNull in joined.DefaultIfEmpty()
                     select new { ent, eOrNull }; // error: System.InvalidOperationException: Nullable object must have a value.
            /*
            -- @__p_0={ '0', '1', '1000' } (DbType = Object)
            SELECT e."Id", e."Repeat2", e."Repeat4", e."UniqValue", p.value AS "eOrNull"
            FROM "EntityWithValues" AS e
            LEFT JOIN unnest(@__p_0) AS p(value) ON e."Repeat2" = p.value
             */


            var q3 = from e in es
                     join ent in context.EntityWithValues on e equals ent.Repeat2 into joined
                     from entOrNull in joined.DefaultIfEmpty()
                     select new { entOrNull, e }; // enumerable, no async

            var q4 = from ent in context.EntityWithValues
                     join e in es on ent.Repeat2 equals e
                     select new { ent, e };


            var q4Res = q4.AsAsyncEnumerable();

            return Ok(q3.ToArray());
        }
    }

}
