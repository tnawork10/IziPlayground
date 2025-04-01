using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Service.Master.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PartialFlushController : ControllerBase
    {


        [HttpGet(nameof(GetLongs))]
        public async Task GetLongs([FromQuery] int count)
        {
            Response.ContentType = "multipart/mixed; boundary=boundary123";
            await using var writer = new StreamWriter(Response.Body, Encoding.UTF8, leaveOpen: true);
            for (int i = 0; i < count; i++)
            {
                await writer.WriteLineAsync("--boundary123");
                await writer.WriteLineAsync("Content-Type: application/json");
                await writer.WriteLineAsync();
                await writer.WriteLineAsync(JsonSerializer.Serialize(i + 100));
                await writer.FlushAsync(); // Flush each part immediately
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            await writer.WriteLineAsync("--boundary123--"); // End boundary
            await writer.FlushAsync();
        }

        [HttpPost(nameof(PostLongs))]
        public async Task PostLongs([FromBody] IEnumerable<long> longs)
        {
            Response.ContentType = "multipart/mixed; boundary=boundary123";
            await using var writer = new StreamWriter(Response.Body, Encoding.UTF8, leaveOpen: true);
            for (int i = 0; i < longs.Count(); i++)
            {
                await writer.WriteLineAsync("--boundary123");
                await writer.WriteLineAsync("Content-Type: application/json");
                await writer.WriteLineAsync();
                await writer.WriteLineAsync(JsonSerializer.Serialize(i + 100));
                await writer.FlushAsync(); // Flush each part immediately
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            await writer.WriteLineAsync("--boundary123--"); // End boundary
            await writer.FlushAsync();
        }
    }
}
