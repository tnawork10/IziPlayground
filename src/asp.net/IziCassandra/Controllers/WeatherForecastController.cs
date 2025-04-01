using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Cassandra;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ZnModelModule.OpenTelemetry;
using ZnModelModule.Shared.InternalCassandra.Storage;
using F = System.IO.File;

namespace IziCassandra.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public const int port = 9090;
        public const string user = "user";
        public const string pwd = "user";
        private Cassandra.ISession session;

        [HttpGet("CreateConnection")]
        public async Task<IActionResult> CreateConnection()
        {
            var builder = Cluster.Builder();
            IRetryPolicy policy = default;
            this.session = builder
              .WithPort(port)
              .WithQueryTimeout(300000)
              .WithCredentials(user, pwd)
              //.WithRetryPolicy(policy)
              //.WithPoolingOptions(poolingOptions)
              .Build()
              .Connect();

            return NoContent();
        }

        [HttpGet("ExecuteQ")]
        public async Task<IActionResult> ExecuteQ()
        {
            await session.ExecuteAsync(new SimpleStatement());
            return NoContent();
        }

        [HttpGet("GetWeatherForecast")]
        public async Task<IActionResult> GetWeatherForecast()
        {
            var context = Activity.Current?.Context ?? default(ActivityContext);
            using var root = ZnModuleActivity.ActivitySource.StartActivity(nameof(GetWeatherForecast), ActivityKind.Server, context);

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
    }
}
