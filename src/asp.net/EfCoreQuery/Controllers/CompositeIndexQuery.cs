using IziHardGames.Playgrounds.ForEfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EfCoreQuery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompositeIndexQuery(QueryDbContext context, NpgsqlConnectionStringBuilder csBuilder,  IServiceProvider serviceProvider) : ControllerBase
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
    }
}
