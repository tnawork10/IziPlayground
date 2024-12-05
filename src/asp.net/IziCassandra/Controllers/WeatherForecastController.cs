using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ZnModelModule.OpenTelemetry;
using ZnModelModule.Shared.InternalCassandra.Storage;
using F = System.IO.File;

namespace IziCassandra.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        [HttpGet("GetWeatherForecast")]
        public async Task<IActionResult> GetWeatherForecast()
        {
            var context = Activity.Current?.Context ?? default(ActivityContext);
            using var root = ZnModuleActivity.ActivitySource.StartActivity(nameof(GetWeatherForecast), ActivityKind.Server, context);

            var v = new SequenceAggregator();
            //var text = await F.ReadAllTextAsync(@"C:\Users\ngoc\Downloads\Telegram Desktop\1d4a4d44-408b-427e-b1e2-d3c52d80b347");
            var text = await F.ReadAllTextAsync(@"C:\Users\ivan\Downloads\Telegram Desktop\1d4a4d44-408b-427e-b1e2-d3c52d80b347");
            using var act0 = ZnModuleActivity.ActivitySource.StartActivity("ReadFile", ActivityKind.Server, root?.Context ?? default);
            var input = JsonSerializer.Deserialize<Serilized[]>(text);
            var list = new List<Task<ValueAtDateSequence>>();
            act0?.Stop();

            for (int i = 0; i < 50; i++)
            {
                var act1 = ZnModuleActivity.ActivitySource.StartActivity($"Execute {i}", ActivityKind.Server, root?.Context ?? default);

                var task = Task.Run(() =>
                {
                    var subAct0 = ZnModuleActivity.ActivitySource.StartActivity($"Queue {i}", ActivityKind.Server, act1?.Context ?? default);
                    var seqH = new SequenceHourly(input.Select(x => ValueAtDate.FromMilliseconds((long)(new TimeSpan(x.DateTime.Ticks).TotalMilliseconds), x.Value)));
                    var daily = seqH.GetDailyFromAvaregedHourly();
                    var seq = daily.ToSequenceAsSum();
                    subAct0?.Stop();
                    var subAct1 = ZnModuleActivity.ActivitySource.StartActivity($"Materialize {i}", ActivityKind.Server, act1?.Context ?? default);
                    subAct1?.SetTag("custom.color", "green");
                    var mat = seq.MaterializeToArray();
                    subAct1?.Stop();
                    act1?.Stop();
                    return mat;

                });
                list.Add(task);
            }
            await Task.WhenAll(list);
            return NoContent();
        }
    }

    public class Serilized
    {
        public DateTime DateTime { get; set; }
        public double? Value { get; set; }
    }

}
