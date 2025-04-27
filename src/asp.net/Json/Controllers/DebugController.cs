using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Json.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DebugController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var model = new JsonModel(101, float.MaxValue / 2);
            var json = JsonSerializer.Serialize(model);
            var jsonPascal = JsonSerializer.Serialize(model, new JsonSerializerOptions() { PropertyNamingPolicy = null });
            var jsonCamle = JsonSerializer.Serialize(model, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var restored = JsonSerializer.Deserialize<JsonModel>(json);
            var restoredPascal = JsonSerializer.Deserialize<JsonModel>(jsonPascal);
            var restoredCamel = JsonSerializer.Deserialize<JsonModel>(jsonCamle);
            return Ok(restored);
        }
    }

    public class JsonModel
    {
        public int Id { get; }
        public int SomeInt { get; set; }
        public required float SomeFloat { get; set; }
        public double SomeDouble { get; set; }
        public DateTime DateTimeReq { get; set; }
        public DateTime? DateTimeNullable { get; set; }

        [JsonConstructor, SetsRequiredMembers]
        public JsonModel(int id, float somefloat)
        {
            Id = id;
            SomeFloat = somefloat;
        }


        public JsonModel(DateTime req, float someF)
        {
            Id = int.MaxValue;
            DateTimeReq = req;
            SomeFloat = someF;
        }
    }
}
