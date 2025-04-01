using System.Linq;
using IziHardGames.Playgrounds.ForEfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EfCoreQuery.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CompositeIndexQuery(QueryDbContext context, NpgsqlConnectionStringBuilder csBuilder, IServiceProvider serviceProvider) : ControllerBase
    {
        /// <summary>
        /// Не сработает
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(WhereAnonKey))]
        public async Task<IActionResult> WhereAnonKey()
        {
            var keys = new List<(int, int)>();
            for (int i = 1; i < 20; i += 2)
            {
                for (int j = 0; j < 20; j += 3)
                {
                    keys.Add((i, j));
                }
            }
            var anonKeys = keys.Select(x => new { UniqIndex1 = x.Item1, UniqIndex2 = x.Item2 }).ToList();
            var q = context.EntityWithCompositeUniqIndices.Where(x => anonKeys.Contains(new { x.UniqIndex1, x.UniqIndex2 }));
            var res = await q.ToArrayAsync();
            return Ok(new { Qs = q.ToQueryString(), Content = res });
        }

        /// <summary>
        /// Не сработает
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(WhereAnonKey2))]
        public async Task<IActionResult> WhereAnonKey2()
        {
            var keys = new List<(int, int)>();
            for (int i = 1; i < 20; i += 2)
            {
                for (int j = 0; j < 20; j += 3)
                {
                    keys.Add((i, j));
                }
            }
            var anonKeys = keys.Select(x => new { UniqIndex1 = x.Item1, UniqIndex2 = x.Item2 }).ToList();
            var q = context.EntityWithCompositeUniqIndices.Where(x => anonKeys.Any(y => x.UniqIndex1 == x.UniqIndex1 && x.UniqIndex2 == y.UniqIndex2));
            var res = await q.ToArrayAsync();
            return Ok(new { Qs = q.ToQueryString(), Content = res });
        }
        /// <summary>
        /// Не сработает
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(WhereCompiled))]
        public async Task<IActionResult> WhereCompiled()
        {
            var keys = new List<(int, int)>();
            for (int i = 1; i < 20; i += 2)
            {
                for (int j = 0; j < 20; j += 3)
                {
                    keys.Add((i, j));
                }
            }
            var anonKeys = keys.Select(x => new { UniqIndex1 = x.Item1, UniqIndex2 = x.Item2 }).ToList();
            var typesKeys = keys.Select(x => new KeyTyped(x.Item1, x.Item2)).ToList();
            var q = await QueryDbContext.CompiledV1(context, typesKeys);
            return Ok(q);
        }

        /// <summary>
        /// Не сработает (is not supported for parameters having no NpgsqlDbType or DataTypeName. Try setting one of these values to the expected database type..)
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(Where5))]
        public async Task<IActionResult> Where5()
        {
            var keys = new List<(int, int)>();
            for (int i = 1; i < 20; i += 2)
            {
                for (int j = 0; j < 20; j += 3)
                {
                    keys.Add((i, j));
                }
            }
            var anonKeys = keys.Select(x => new { UniqIndex1 = x.Item1, UniqIndex2 = x.Item2 }).ToList();
            var typesKeys = keys.Select(x => new KeyTyped(x.Item1, x.Item2)).ToList();
            List<(int UniqIndex1, int UniqIndex2)> asList = anonKeys.Select(x => (x.UniqIndex1, x.UniqIndex2)).ToList();
            var q = context.EntityWithCompositeUniqIndices.Where(x => asList.Contains(new(x.UniqIndex1, x.UniqIndex2)));
            //var q = await QueryDbContext.CompiledV1(context, typesKeys);
            return Ok(q);
        }
        /// <summary>
        /// Не сработает (is not supported for parameters having no NpgsqlDbType or DataTypeName. Try setting one of these values to the expected database type..)
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(WhereValueTuple))]
        public async Task<IActionResult> WhereValueTuple()
        {
            var keys = new List<(int, int)>();
            for (int i = 1; i < 20; i += 2)
            {
                for (int j = 0; j < 20; j += 3)
                {
                    keys.Add((i, j));
                }
            }
            var anonKeys = keys.Select(x => new { UniqIndex1 = x.Item1, UniqIndex2 = x.Item2 }).ToList();
            var typesKeys = keys.Select(x => new KeyTyped(x.Item1, x.Item2)).ToList();
            List<ValueTuple<int, int>> asList = anonKeys.Select(x => (x.UniqIndex1, x.UniqIndex2)).ToList();
            var q = context.EntityWithCompositeUniqIndices.Where(x => asList.Contains(new(x.UniqIndex1, x.UniqIndex2)));
            //var q = await QueryDbContext.CompiledV1(context, typesKeys);
            return Ok(q);
        }
        /// <summary>
        /// Не сработает (is not supported for parameters having no NpgsqlDbType or DataTypeName. Try setting one of these values to the expected database type..)
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(Cortage))]
        public async Task<IActionResult> Cortage()
        {
            var keys = new List<(int, int)>();
            for (int i = 1; i < 20; i += 2)
            {
                for (int j = 0; j < 20; j += 3)
                {
                    keys.Add((i, j));
                }
            }

            var anonKeys = keys.Select(x => new { UniqIndex1 = x.Item1, UniqIndex2 = x.Item2 }).ToList();
            var typesKeys = keys.Select(x => new KeyTyped(x.Item1, x.Item2)).ToList();
            List<(int UniqIndex1, int UniqIndex2)> asList = anonKeys.Select(x => (x.UniqIndex1, x.UniqIndex2)).ToList();
            //var q = await QueryDbContext.CompiledV1(context, typesKeys);
            return Ok(asList.GetType().AssemblyQualifiedName);
        }

        [HttpPost(nameof(ValueTupleCreate))]
        public async Task<IActionResult> ValueTupleCreate()
        {
            var keys = new List<(int, int)>();
            for (int i = 1; i < 20; i += 2)
            {
                for (int j = 0; j < 20; j += 3)
                {
                    keys.Add((i, j));
                }
            }

            var q = context.EntityWithCompositeUniqIndices.Where(x => keys.Any(y => y.Equals(ValueTuple.Create(x.UniqIndex1, x.UniqIndex2))));
            var res = await q.ToArrayAsync();
            return Ok(res);
        }

        /// <summary>
        /// вытаскивает больше чем нужно
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(WhereChain))]
        public async Task<IActionResult> WhereChain()
        {
            var keys = new[] { (5, 5), (1, 1) }.ToList();
            var k1 = keys.Select(x => x.Item1);
            var k2 = keys.Select(x => x.Item2);
            //var q = context.CompositeKeyJoins.Where(x => keys.Contains(ValueTuple.Create(x.IdPart1, x.IdPart2)));
            var q = context.CompositeKeyJoins.Where(x => k1.Contains(x.IdPart1)).Where(x => k2.Contains(x.IdPart2)); //.Select(x => x.KeyTyped);
            var res = await q.ToArrayAsync();
            return Ok(res);
        }

        [HttpPost(nameof(CreateType))]
        public async Task<IActionResult> CreateType()
        {
            var keys = new[] { (5, 5), (1, 1) };
            var typedList = new List<CompositeKeyJoin>();
            foreach (var item in keys)
            {
                typedList.Add(new CompositeKeyJoin() { IdPart1 = item.Item1, IdPart2 = item.Item2 });
            }
            //var q = context.CompositeKeyJoins.Union(typedList); // error

            // System.AggregateException: One or more errors occurred. (Cannot translate 'Contains' on a subquery expression of entity type 'CompositeKeyJoin' because it has a composite primary key. See https://go.microsoft.com/fwlink/?linkid=2141942 for information on how to rewrite your query.)
            //var q = context.CompositeKeyJoins.Where(x => typedList.Contains(x));

            var q = context.CompositeKeyJoins.Where(x => typedList.Any(y => y.IdPart1 == x.IdPart1 && y.IdPart2 == x.IdPart2)); // error
            var res = q.ToArrayAsync();
            return Ok(res);
        }


        [HttpPost(nameof(AsSingleQuery))]
        public async Task<IActionResult> AsSingleQuery()
        {
            var keys = new[] { (5, 5), (1, 1) };
            var q = context.CompositeKeyJoins.Take(0);

            foreach (var item in keys)
            {
                q = q.Union(context.CompositeKeyJoins.Where(x => x.IdPart1 == item.Item1 && x.IdPart2 == item.Item2));
            }
            var qs = q.ToQueryString();
            q = q.AsSingleQuery();
            var qsSingle = q.ToQueryString();
            var res = await q.ToArrayAsync();
            return Ok(new { qs = qs, qsSingle = qsSingle, res = res });
        }

        [HttpPost(nameof(AsMultipleQuery))]
        public async Task<IActionResult> AsMultipleQuery()
        {
            var keys = new[] { (5, 5), (1, 1) };
            var q = context.CompositeKeyJoins.Take(0);

            foreach (var item in keys)
            {
                q = q.Union(context.CompositeKeyJoins.Where(x => x.IdPart1 == item.Item1 && x.IdPart2 == item.Item2));
            }
            var qs = q.ToQueryString();
            var res = await q.ToArrayAsync();
            return Ok(new { qs = qs, res = res });
        }
    }
}
