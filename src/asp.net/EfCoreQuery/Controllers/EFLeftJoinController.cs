using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using IziHardGames.Playgrounds.ForEfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Npgsql;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EfCoreQuery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EFLeftJoinController : ControllerBase
    {
        private QueryDbContext context;
        private NpgsqlConnectionStringBuilder csBuilder;

        public EFLeftJoinController(QueryDbContext context, NpgsqlConnectionStringBuilder csBuilder)
        {
            this.context = context;
            this.csBuilder = csBuilder;
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
            var anonAsArray = range.Select(x => new { x.IdPart1, x.IdPart2 }).ToArray();
            var objsAsArrat = range.ToArray();

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

            //var v25 = context.CompositeKeyJoins.Where(x => );

            var keys = new List<(int, int)>() { (2, 2), (4, 4) };
            var hashset = new List<(int, int)>() { (2, 2), (4, 4) }.Select(x => new { x.Item1, x.Item2 }).ToHashSet();
            var keysAsAnon = new List<TypeCompositeKeyObj>() { new TypeCompositeKeyObj() { IdPart1 = 5, IdPart2 = 5 }, new TypeCompositeKeyObj() { IdPart1 = 4, IdPart2 = 4 } };

            if (false)
            {   // 
                //var vv = await context.CompositeKeyJoins.Where(x => keys.Any(y => y.Item1 == x.IdPart1 && y.Item2 == x.IdPart2)).ToArrayAsync(); // error
                //var vv = await context.CompositeKeyJoins.Where(x => keysAsAnon.Any(y => y.IdPart1 == x.IdPart1 && y.IdPart2 == x.IdPart2)).ToArrayAsync(); // error
            }
            //var vv = await context.CompositeKeyJoins.Where(x => hashset.Contains(new(x.IdPart1, x.IdPart2))).ToArrayAsync(); //  error
            //var vv = await context.CompositeKeyJoins.Where(x => hashset.Contains(new { Item1 = x.IdPart1, Item2 = x.IdPart2 })).ToArrayAsync(); // error


            //var q = context.CompositeKeyJoins.Select(x => new { Key = new { x.IdPart1, x.IdPart2 }, Val = x.Value }).Join(e2sKeys, x => x.Key, y => y, (x, y) => new { x, y }); // error
            //var q = context.CompositeKeyJoins.AsQueryable().LeftJoin(e2sKeys, x => ValueTuple.Create(x.IdPart1, x.IdPart2), y => ValueTuple.Create(y.IdPart1, y.IdPart2), (x, y) => x); // error
            var tuple = ValueTuple.Create(5, 5);
            var arrOfTuple = new ValueTuple<int, int>[] { tuple };
            //var tuple1 = context.CompositeKeyJoins.Where(x => x.IdPart1 == tuple.Item1 && x.IdPart2 == tuple.Item2).FirstOrDefault(); // ok
            //var tuple2 = context.CompositeKeyJoins.Where(x => arrOfTuple.Any(z => z.Item1 == x.IdPart1 && z.Item2 == x.IdPart2)).FirstOrDefault(); // error
            //var tuple3 = context.CompositeKeyJoins.Where(x => arrOfTuple.Contains(ValueTuple.Create(x.IdPart1, x.IdPart2))).FirstOrDefault(); // error
            var tuple4 = context.CompositeKeyJoins.Where(x => objsAsArrat.Any(y => y.IdPart1 == x.IdPart1 && y.IdPart2 == x.IdPart2)).FirstOrDefault(); // error


            //return Ok(vv);
            //return Ok(await q.ToArrayAsync());
            //return Ok(q);
            //return Ok(await query.ToArrayAsync());
            //return Ok(await v23.ToArrayAsync());
            //return Ok(result);
            return NoContent();
        }

        [HttpPost(nameof(CompositeJoinAtRemote))]
        public async Task<IActionResult> CompositeJoinAtRemote()
        {
            /*
              explain analyze    
              SELECT c.id_part1, c.id_part2, c.some_random_value, c.value
              FROM composite_key_joins AS c
              INNER JOIN composite_key_joins_keys AS c0 ON c.id_part1 = c0.id_part1 AND c.id_part2 = c0.id_part2
              LIMIT 100

Limit  (cost=0.30..7.90 rows=100 width=32) (actual time=0.045..0.309 rows=100 loops=1)
  ->  Nested Loop  (cost=0.30..746.02 rows=9801 width=32) (actual time=0.044..0.303 rows=100 loops=1)
        ->  Seq Scan on composite_key_joins_keys c0  (cost=0.00..161.01 rows=9801 width=8) (actual time=0.004..0.008 rows=100 loops=1)
        ->  Memoize  (cost=0.30..0.36 rows=1 width=32) (actual time=0.003..0.003 rows=1 loops=100)
              Cache Key: c0.id_part1, c0.id_part2
              Cache Mode: logical
              Hits: 0  Misses: 100  Evictions: 0  Overflows: 0  Memory Usage: 14kB
              ->  Index Scan using pk_composite_key_joins on composite_key_joins c  (cost=0.29..0.35 rows=1 width=32) (actual time=0.002..0.002 rows=1 loops=100)
                    Index Cond: ((id_part1 = c0.id_part1) AND (id_part2 = c0.id_part2))
            Planning Time: 0.286 ms
            Execution Time: 0.388 ms
             */
            var e2s = await context.CompositeKeyJoins.Join(context.CompositeKeyJoinsKeys, x => new { x.IdPart1, x.IdPart2 }, y => new { y.IdPart1, y.IdPart2 }, (x, y) => x).Take(100).ToArrayAsync();
            return Ok(e2s);
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

        [HttpPost(nameof(CompositeKeyWithRawQuery))]
        public async Task<IActionResult> CompositeKeyWithRawQuery()
        {
            //var fs = new FormattableString();
            var q = context.CompositeKeyJoins.FromSqlRaw("select * from \"composite_key_joins\" limit 50");
            //var q2 = context.CompositeKeyJoinsKeys.FromSqlInterpolated($"{1}");
            return Ok(await q.ToArrayAsync());
        }

     
        [HttpPost(nameof(CompositeKeyWithRawQueryExt))]
        public async Task<IActionResult> CompositeKeyWithRawQueryExt()
        {
            var e2 = await context.CompositeKeyJoins.Take(100).ToArrayAsync();
            var keys = e2.Select(x => (x.IdPart1, x.IdPart2));


            var q = context.CompositeKeyJoins.JoinByCompositeKey(
                keys,
                "composite_key_joins",
                "id_part1",
                "id_part2").AsNoTracking();
            return Ok(await q.ToArrayAsync());
        }
        [HttpPost(nameof(CompositeKeyWithRawQueryExtV1))]
        public async Task<IActionResult> CompositeKeyWithRawQueryExtV1()
        {
            var e2 = await context.CompositeKeyJoins.Take(100).ToArrayAsync();
            var keys = e2.Select(x => (x.IdPart1, x.IdPart2));
            var q = context.CompositeKeyJoins.JoinByCompositeKeyV1(
                keys,
                "composite_key_joins",
                "id_part1",
                "id_part2").AsNoTracking();
            return Ok(await q.ToArrayAsync());
        } 
        [HttpPost(nameof(CompositeKeyWithRawQueryExtV2))]
        public async Task<IActionResult> CompositeKeyWithRawQueryExtV2()
        {
            var e2 = await context.CompositeKeyJoins.Take(100).ToArrayAsync();
            var keys = e2.Select(x => (x.IdPart1, x.IdPart2));
            var q = context.CompositeKeyJoins.JoinByCompositeKeyV2(
                keys,
                "composite_key_joins",
                "id_part1",
                "id_part2").AsNoTracking();
            return Ok(await q.ToArrayAsync());
        } 
        
        [HttpPost(nameof(CompositeKeyWithRawQueryExtV3))]
        public async Task<IActionResult> CompositeKeyWithRawQueryExtV3()
        {
            var e2 = await context.CompositeKeyJoins.Take(100).ToArrayAsync();
            var keys = e2.Select(x => (x.IdPart1, x.IdPart2));
            var q = context.CompositeKeyJoins.JoinByCompositeKeyV3(
                keys,
                "composite_key_joins",
                "id_part1",
                "id_part2").AsNoTracking();
            return Ok(await q.ToArrayAsync());
        }


        [HttpPost(nameof(CompositeKeyWithLinqExpression))]
        public async Task<IActionResult> CompositeKeyWithLinqExpression()
        {
            var e2 = await context.CompositeKeyJoins.Take(100).ToArrayAsync();
            var keys = e2.Select(x => (x.IdPart1, x.IdPart2));
            var q = context.CompositeKeyJoins.CompositeKeyFilterP2(keys, nameof(CompositeKeyJoin.IdPart1), nameof(CompositeKeyJoin.IdPart2));
            //var q2 = context.CompositeKeyJoinsKeys.FromSqlInterpolated($"{1}");
            return Ok(await q.ToArrayAsync());
        }

        [HttpPost(nameof(DapperQuery))]

        public async Task<IActionResult> DapperQuery()
        {

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            //    const string sql = @"SELECT *
            //FROM ""CompositeKeyJoins""
            //WHERE ""IdPart1"" IN (@CompositeKeys);";

            const string sql = @"SELECT *
        FROM composite_key_joins
        WHERE id_part1 = ANY (@aaa);";
            //var keys = new[] { new TypeCompositeKeyObj { IdPart1 = 5, IdPart2 = 5 } };
            var keys = new[] { 5 };


            // Convert to a structure that Dapper can understand as an array of parameters
            //var parameterList = compositeKeys.Select(key => new { key.ColumnA, key.ColumnB });

            using (IDbConnection db = new NpgsqlConnection(csBuilder.ConnectionString))
            {
                var q = db.QueryAsync<CompositeKeyJoin>(sql, new
                {
                    aaa = keys
                });
                return Ok(await q);
            }
        }
        [HttpPost(nameof(DapperQueryComplex))]

        public async Task<IActionResult> DapperQueryComplex()
        {

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            //    const string sql = @"SELECT *
            //FROM ""CompositeKeyJoins""
            //WHERE ""IdPart1"" IN (@CompositeKeys);";

            const string sql = @"SELECT *
        FROM composite_key_joins
        WHERE (id_part1,id_part2) = ANY (@aaa);";
            //WHERE (id_part1,id_part2) = ANY (@aaa);"; // error
            //var keys = new[] { new TypeCompositeKeyObj { IdPart1 = 5, IdPart2 = 5 } };
            //var keys = new[] { (5, 5) };    // error
            var keys = new[] { new { id_part1 = 5, id_part2 = 5 } };    // error


            // Convert to a structure that Dapper can understand as an array of parameters
            //var parameterList = compositeKeys.Select(key => new { key.ColumnA, key.ColumnB });

            using (IDbConnection db = new NpgsqlConnection(csBuilder.ConnectionString))
            {
                var q = db.QueryAsync<CompositeKeyJoin>(sql, new
                {
                    aaa = keys
                });
                return Ok(await q);
            }
        }
    }
}
