using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Index.HPRtree;

namespace DevextremeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DevextremeController : ControllerBase
    {
        [HttpPost("GetDatas")]
        public async Task<IActionResult> GetDatas([FromBody] JsonObject body)
        {
            var datas = new DevExtremeRecord[10];
            for (int i = 0; i < 10; i++)
            {
                datas[i] = new DevExtremeRecord()
                {
                    MyId = i,
                    ValueAsDouble = i + (i / 3d) / double.MaxValue,
                    ValueAsInt = Guid.NewGuid().GetHashCode(),
                    ValueAsString = DateTime.Now.ToString(),
                };
            }
            return Ok(new { totalCount = 100, data = datas, body = body });
        }
    }

    public class DevExtremeRecord
    {
        public int MyId { get; set; }
        public string ValueAsString { get; set; }
        public double ValueAsDouble { get; set; }
        public int ValueAsInt { get; set; }
    }
}