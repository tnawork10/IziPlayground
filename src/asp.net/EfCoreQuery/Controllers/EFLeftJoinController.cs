using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        [HttpPost(nameof(CompositeJoin))]
        public async Task<IActionResult> CompositeJoin()
        {
            var list = new List<(int, int)>();
            for (int i = 1; i < 5; i++)
            {
                for (int j = 1; j < 5; j++)
                {
                    list.Add((i, j));
                }
            }
            //var q = context.CompositeKeyJoins.Join(list, x => new { x.IdPart1, x.IdPart2 }, y => new { IdPart1 = y.Item1, IdPart2 = y.Item2 }, (x, y) => x); // error
            //var q = context.CompositeKeyJoins.Join(list.Select(y => new { IdPart1 = y.Item1, IdPart2 = y.Item2 }), x => new { x.IdPart1, x.IdPart2 }, y => y, (x, _) => x); // error
            //var q = context.CompositeKeyJoins.IntersectBy(list, x => new(x.IdPart1, x.IdPart2));    // error
            //var q = context.CompositeKeyJoins.IntersectBy(list.Select(x => new { IdPart1 = x.Item1, IdPart2 = x.Item2 }), x => new { x.IdPart1, x.IdPart2 });   // error
            //var q = context.CompositeKeyJoins.IntersectBy();   // error


            //return Ok(await q.ToArrayAsync());
            return NoContent();
        }

        [HttpPost(nameof(CompositeFind))]
        public async Task<IActionResult> CompositeFind()
        {
            // Ok
            var finded = await context.CompositeKeyJoins.FindAsync(5, 5);
            return Ok(finded); // error
            return Ok(await context.CompositeKeyJoins.FindAsync(new { IdPart1 = 5, IdPart2 = 5 })); // error
        }
        [HttpPost(nameof(CompositeWhereProp))]
        public async Task<IActionResult> CompositeWhereProp()
        {
            // error
            var result = await context.CompositeKeyJoins.Where(x => x.Key == new { IdPart1 = 5, IdPart2 = 5 }).FirstOrDefaultAsync();
            return Ok(result);
        }

        [HttpPost(nameof(CompositeWherePropTyped))]
        public async Task<IActionResult> CompositeWherePropTyped()
        {
            // error
            //var result = await context.CompositeKeyJoins.Where(x => x.KeyTyped == new KeyTyped(5, 5)).FirstOrDefaultAsync();
            var result = await context.CompositeKeyJoins.Where(x => x.IdPart1 == 5 && x.IdPart2 == 5).FirstOrDefaultAsync();    // Ok
            return Ok(result);
        }


        [HttpPost(nameof(CompositeJoinWithEntity))]
        public async Task<IActionResult> CompositeJoinWithEntity()
        {
            var e2s = await context.CompositeKeyJoinsKeys.Take(100).ToArrayAsync();

            //var result = await context.CompositeKeyJoins.Join(e2s, x => new { x.IdPart1, x.IdPart2 }, y => new { y.IdPart1, y.IdPart2 }, (x, y) => new { x, y }).ToArrayAsync(); // error
            //var result = await context.CompositeKeyJoins.Join(e2s, x => new { x.IdPart1, x.IdPart2 }, y => new { y.IdPart1, y.IdPart2 }, (x, y) => new { x, y }).ToListAsync(); // error
            //var result = context.CompositeKeyJoins.Join(e2s, x => new { x.IdPart1, x.IdPart2 }, y => new { y.IdPart1, y.IdPart2 }, (x, y) => new { x, y }).AsEnumerable().ToArray(); // error

            //var result = await context.CompositeKeyJoins.Join(e2s, x => x.IdPart1, y => y.IdPart1, (x, y) => new { x, y }).ToArrayAsync(); // error
            //var result = await context.CompositeKeyJoins.Join(e2s, x => x.IdPart1, y => y.IdPart1, (x, y) => x).Join(e2s, x => x.IdPart2, y => y.IdPart2, (x, y) => new { x, y }).ToArrayAsync(); // error

            //var q = from e in context.CompositeKeyJoins
            //        join e2 in e2s on new { e.IdPart1, e.IdPart2 } equals new { e2.IdPart1, e2.IdPart2 }
            //        select new { e, e2 }; // error



            //var result = await q.ToArrayAsync();

            //var result = await context.CompositeKeyJoins.Where(x => e2s.Any(y => x.IdPart1 == y.IdPart1 && x.IdPart2 == y.IdPart2)).ToListAsync(); // error
            ///var result = await context.CompositeKeyJoins.Where(x => e2s.Any(y => x.IdPart1 == y.IdPart1 && x.IdPart2 == y.IdPart2)).ToListAsync(); // error


            //var result = await context.CompositeKeyJoins.Join(e2s, x => x.IdPart1, y => y.IdPart1, (x, y) => x)
            //                                            .Join(e2s, x => x.IdPart2, y => y.IdPart2, (x, y) => new { x, y })
            //                                            .ToArrayAsync(); // error
            var p = new { IdPart1 = 5, IdPart2 = 5 };
            var e2sKeys = e2s.Select(x => new { IdPart1 = x.IdPart1, x.IdPart2 }).AsQueryable();
            var e2sKeysTuple = e2s.Select(x => (x.IdPart1, x.IdPart2));
            var e2sQ = e2s.AsQueryable();
            var e2sEn = e2s.AsEnumerable();
            var range = e2s.Select(x => new CompositeKeyJoin()
            {
                IdPart1 = x.IdPart1,
                IdPart2 = x.IdPart2,
            }).AsQueryable();

            //var q = context.CompositeKeyJoins.Where(x => e2sKeys.Contains(new { x.IdPart1, x.IdPart2 }));    // error

            //var query = from x in context.CompositeKeyJoinsKeys
            //            join y in context.CompositeKeyJoins on new { x.IdPart1, x.IdPart2 } equals new { y.IdPart1, y.IdPart2 }
            //            select new { x, y };    // OK

            //var query = from x in context.CompositeKeyJoinsKeys
            //            join y in e2s on new { x.IdPart1, x.IdPart2 } equals new { y.IdPart1, y.IdPart2 }
            //            select new { x, y };    // Error

            if (false)
            {
                //var query = from x in e2s
                //            join y in context.CompositeKeyJoinsKeys on new { x.IdPart1, x.IdPart2 } equals new { y.IdPart1, y.IdPart2 }
                //            select new { x, y };    // гребет все с бэка 
                /*
                SELECT c."Id", c."IdPart1", c."IdPart2"
                FROM "CompositeKeyJoinsKeys" AS c
                */
            }

            //var query = from x in e2s.AsQueryable()
            //            join y in context.CompositeKeyJoinsKeys on new { x.IdPart1, x.IdPart2 } equals new { y.IdPart1, y.IdPart2 }
            //            select new { x, y }; // error: System.ArgumentException: must be reducible node



            //var q = context.CompositeKeyJoins.Join(, x => new { x.IdPart1, x.IdPart2 }, y => p, (x, y) => x);
            //var result = await q.ToArrayAsync();
            //var result = await query.Take(5).ToArrayAsync();


            //var query = from x in context.CompositeKeyJoinsKeys
            //            join y in e2s on  x.IdPart1 equals  y.IdPart1
            //            where x.IdPart2 == y.IdPart2
            //            select new { x, y };    //error

            //var query = from x in context.CompositeKeyJoins
            //            join y in range on new { x.IdPart1, x.IdPart2 } equals new { y.IdPart1, y.IdPart2 }
            //            select new { x, y };    // error

            //var query = from x in context.CompositeKeyJoins
            //            from y in range
            //            where new { x.IdPart1, x.IdPart2 } == new { y.IdPart1, y.IdPart2 }
            //            select new { x, y };    // error

            //var query = from x in context.CompositeKeyJoins
            //            from y in range
            //            where x.IdPart1 == y.IdPart1 && x.IdPart2 == y.IdPart2
            //            select new { x };    // error

            //var query = from x in context.CompositeKeyJoins
            //            from y in e2sQ
            //            where x.IdPart1 == y.IdPart1 && x.IdPart2 == y.IdPart2
            //            select new { x };    // error

            if (false)
            {
                // error
                var v1 = context.CompositeKeyJoins.Join(e2sQ, x => x.IdPart1, y => y.IdPart1, (x, y) => x);
                var v2 = context.CompositeKeyJoins.Join(e2sQ, x => x.IdPart2, y => y.IdPart2, (x, y) => x);
                var v3 = v1.Intersect(v2);
                return Ok(v3.ToArray());
            }

            if (false)
            {
                //var result = context.CompositeKeyJoins.CompositeJoinP2(e2sKeysTuple, nameof(CompositeKeyJoin.IdPart1), nameof(CompositeKeyJoin.IdPart2));
            }
            var keys1 = e2s.Select(x => x.IdPart1).ToArray();
            var keys2 = e2s.Select(x => x.IdPart2).ToArray();
            if (false)
            {
                // очень долго и некправильно так как между запросам ИЛИ
                var v21 = context.CompositeKeyJoins.Where(x => keys1.Contains(x.IdPart1)).Where(x => keys2.Contains(x.IdPart2));
                /*
                    SELECT c."IdPart1", c."IdPart2", c."SomeRandomValue", c."Value"
                    FROM "CompositeKeyJoins" AS c
                    WHERE c."IdPart1" = ANY (@__keys1_0) AND c."IdPart2" = ANY (@__keys2_1)
                    тоже очень долго - и неправильно
                 */
                var v22 = context.CompositeKeyJoins.Where(x => keys1.Contains(x.IdPart1) && keys2.Contains(x.IdPart2));
                var v23 = context.CompositeKeyJoins.Where(x => e2sKeys.Contains(new { x.IdPart1, x.IdPart2 })); // error
                var v24 = context.CompositeKeyJoins.Where(x => e2sKeys.Any(x => x == new { x.IdPart1, x.IdPart2 })); // error
            }

            var v25 = context.CompositeKeyJoins.Where(x => );

            //return Ok(q);
            //return Ok(await query.ToArrayAsync());
            return Ok(await v23.ToArrayAsync());
            //return Ok(result);
            return NoContent();
        }

        [HttpPost(nameof(CompositeAttach))]
        public async Task<IActionResult> CompositeAttach()
        {
            var e2s = await context.CompositeKeyJoinsKeys.Take(2).ToArrayAsync();
            var range = e2s.Select(x => new CompositeKeyJoin()
            {
                IdPart1 = x.IdPart1,
                IdPart2 = x.IdPart2,
            });
            context.AttachRange(range);
            await context.SaveChangesAsync();
            return Ok();
        }

    }

    public static class ExtensionsForIQueryable
    {
        /// <summary>
        /// Метод разворачивается в where (item[N].TId1 == x.TId1 || where item[N].TId2 == x.TId2) и так на каждый элемент ids
        /// 
        /*
        Bitmap Heap Scan on "CompositeKeyJoins" c  (cost=12.89..23.14 rows=3 width=32)
          Recheck Cond: ((("IdPart1" = 6) AND ("IdPart2" = 14)) OR (("IdPart1" = 23) AND ("IdPart2" = 9)) OR (("IdPart1" = 94) AND ("IdPart2" = 73)))
          ->  BitmapOr  (cost=12.89..12.89 rows=3 width=0)
        ->  Bitmap Index Scan on "PK_CompositeKeyJoins"  (cost=0.00..4.29 rows=1 width=0)
              Index Cond: (("IdPart1" = 6) AND ("IdPart2" = 14))
        ->  Bitmap Index Scan on "PK_CompositeKeyJoins"  (cost=0.00..4.29 rows=1 width=0)
              Index Cond: (("IdPart1" = 23) AND ("IdPart2" = 9))
        ->  Bitmap Index Scan on "PK_CompositeKeyJoins"  (cost=0.00..4.29 rows=1 width=0)
              Index Cond: (("IdPart1" = 94) AND ("IdPart2" = 73))
        */
        /// </summary>
        public static IQueryable<T> CompositeFilterP2<T, TId1, TId2>(this IQueryable<T> queriable, IEnumerable<(TId1, TId2)> ids, string propName0, string propName1)

        {
            var parameter = Expression.Parameter(typeof(T));

            var body = ids.Select(b => Expression.AndAlso(
                Expression.Equal(Expression.Property(parameter, propName0),
                                 Expression.Constant(b.Item1)),
                Expression.Equal(Expression.Property(parameter, propName1),
                             Expression.Constant(b.Item2))))
            .Aggregate(Expression.OrElse);

            var predicate = Expression.Lambda<Func<T, bool>>(body, parameter);
            return queriable.Where(predicate);
        }
        public static IQueryable<T> CompositeFilterP3<T, TId1, TId2, TId3>(this IQueryable<T> queriable, IEnumerable<(TId1, TId2, TId3)> ids, string propName0, string propName1, string propName3)

        {
            var parameter = Expression.Parameter(typeof(T));

            var body = ids.Select(b => Expression.AndAlso(Expression.AndAlso(
                Expression.Equal(Expression.Property(parameter, propName0),
                                 Expression.Constant(b.Item1)),
                Expression.Equal(Expression.Property(parameter, propName1),
                                Expression.Constant(b.Item2))
                ),
                 Expression.Equal(Expression.Property(parameter, propName3),
                                Expression.Constant(b.Item3))
                ))
            .Aggregate(Expression.OrElse);

            var predicate = Expression.Lambda<Func<T, bool>>(body, parameter);
            return queriable.Where(predicate);
        }
    }
}
