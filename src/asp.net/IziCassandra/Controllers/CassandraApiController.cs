using System.Diagnostics;
using Cassandra;
using IziCassandra.Application;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Instrumentation.Cassandra;
using ZnModelModule.OpenTelemetry;

namespace IziCassandra.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CassandraApiController(CassandraClient client) : ControllerBase
    {
        public const int port = 9090;
        public const string user = "user";
        public const string pwd = "user";
        private Cassandra.ISession session;

        [HttpGet]
        public async Task<IActionResult> CreateConnection()
        {
            var builder = Cluster.Builder();
            IRetryPolicy policy = default;
            this.session = builder
              .WithPort(port)
              .WithQueryTimeout(300000)
              .WithQueryOptions(new QueryOptions().SetPageSize(100000))
              .WithCredentials(user, pwd)
              .WithOpenTelemetryMetrics()
              .Build()
              .Connect();


            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> ExecuteQ()
        {
            await session.ExecuteAsync(new SimpleStatement());
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> TestGet()
        {
            var context = Activity.Current?.Context ?? default(ActivityContext);
            using var root = ZnModuleActivity.ActivitySource.StartActivity(nameof(TestGet), ActivityKind.Server, context);

            var cluster = Cluster.Builder()
                     .AddContactPoints("host1")
                     .Build();


            // Connect to the nodes using a keyspace
            var session = cluster.Connect("sample_keyspace");

            // Execute a query on a connection synchronously
            var rs = session.Execute("SELECT * FROM sample_table");

            // Iterate through the RowSet
            foreach (var row in rs)
            {
                var value = row.GetValue<int>("sample_int_column");

                // Do something with the value
            }
            return NoContent();
        }
        [HttpGet]
        public async Task<IActionResult> TestFromGce()
        {
            var result = await client.QAsync(new DateTime(2024, 08, 01), new DateTime(2024, 09, 01));
            return Ok(result.Select(x => new { x.DateTime, x.value }));
        }

        [HttpGet]
        public async Task<IActionResult> GceReadOnly()
        {
            var count = await client.QReadOnly(new DateTime(2024, 08, 01), new DateTime(2024, 09, 01));
            return Ok(count);
        }

        [HttpGet]
        public async Task<IActionResult> GceReadOnlySharedSession()
        {
            int count = await client.QReadOnlySharedSession(new DateTime(2024, 08, 01), new DateTime(2024, 09, 01));
            return Ok(count);
        }
        [HttpGet]
        public async Task<IActionResult> GceReadOnlySharedSessionPreparedStatement()
        {
            var count = await client.QReadOnlyPrepared(new DateTime(2024, 08, 01), new DateTime(2024, 09, 01));
            return Ok(count);
        }

        [HttpGet]
        public async Task<IActionResult> ReadOnlyParallel([FromQuery] int count = 10)
        {
            var tasks = new List<Task<int>>();
            for (int i = 0; i < count; i++)
            {
                var t1 = Task.Run(async () =>
                 {
                     var count = await client.QReadOnlyPrepared(new DateTime(2024, 08, 01), new DateTime(2024, 09, 01));
                     return count;
                 });
                tasks.Add(t1);
            }
            await Task.WhenAll(tasks);
            return Ok(tasks.Select(x => x.Result).ToArray());
        }


        [HttpGet]
        public async Task<IActionResult> ReadOnlyParallelLimited([FromQuery] int count = 10, int limit = 44640)
        {
            var tasks = new List<Task<int>>();
            for (int i = 0; i < count; i++)
            {
                var t1 = Task.Run(async () =>
                {
                    var count = await client.QReadOnlyPrepared(limit);
                    return count;
                });
                tasks.Add(t1);
            }
            await Task.WhenAll(tasks);
            return Ok(tasks.Select(x => x.Result).ToArray());
        }

        /// <summary>
        /// При count=20 1 инстанс клиента выполнили запрос за 22.1сек а каждый клиент на запрос = 11.073
        /// при этом, используя 1 клиент, время запроса было минимальным, но время read занимало значительную часть времени (~70%).
        /// при использовании отдельных клиентов время выполнения запроса ~5.9сек а время чтения 500-900 милисекунд
        /// потребление памяти с 70 до 370 (отдельные клиенты) и с 70 до 313 (один клиент)
        /// совсем другие результаты из офисной сети (не VPN)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ReadOnlyParallelLimitedSeparate([FromQuery] int count = 10, int limit = 44640)
        {
            var tasks = new List<Task<int>>();
            for (int i = 0; i < count; i++)
            {
                var t1 = Task.Run(async () =>
                {
                    var clientSeparate = new CassandraClient();
                    var count = await clientSeparate.QReadOnlyPrepared(limit);
                    return count;
                });
                tasks.Add(t1);
            }
            await Task.WhenAll(tasks);
            return Ok(tasks.Select(x => x.Result).ToArray());
        }
    }
}
