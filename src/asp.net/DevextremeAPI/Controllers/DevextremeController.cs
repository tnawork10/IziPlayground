using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using IziHardGames.DevExtremeAPI.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DevextremeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DevextremeController(DevExtremeDbContext db, ILogger<DevextremeController> logger) : ControllerBase
    {
        [HttpPost("GetDatas")]
        public async Task<IActionResult> GetDatas([FromBody] JsonElement body)
        {
            logger.LogWarning(body.GetRawText());
            var req = new PagingRequest(body);
            //var datas = new DevExtremeRecord[10];
            //for (int i = 0; i < 10; i++)
            //{
            //    datas[i] = new DevExtremeRecord()
            //    {
            //        Id = i,
            //        ValueAsDouble = i + (i / 3d) / double.MaxValue,
            //        ValueAsInt = Guid.NewGuid().GetHashCode(),
            //        ValueAsString = DateTime.Now.ToString(),
            //    };
            //}
            //return Ok(new { totalCount = 100, data = datas, body = body });
            var total = await db.Records.CountAsync();

            if (req.DataField != null)
            {
                //var values = await db.QueueDistinctsColumn<DevExtremeRecord>(req.DataField, nameof(DevExtremeDbContext.Records));
                //return Ok(new { data = values, totalCount = 100 });

                if (req.DataField == nameof(DevExtremeRecord.Id))
                {
                    var result = await db.Records.Skip(req.Skip).Take(req.Take).OrderBy(x => x.Id).Distinct().Select(x => new { Id = x.Id }).ToArrayAsync();
                    return Ok(new { data = result, totalCount = total });
                }

                var part = await db.Records.Take(10).ToArrayAsync();
                return Ok(new PagingResponse<DevExtremeRecord>()
                {
                    Data = part,
                    TotalCount = total,
                });
            }

            var all = await db.Records.AsNoTracking().Skip(req.Skip).Take(req.Take).OrderBy(x => x.Id).ToArrayAsync();
            return Ok(new PagingResponse<DevExtremeRecord>()
            {
                Data = all,
                TotalCount = total,
            });
        }
    }
    public struct PagingRequest
    {
        public readonly JsonElement el;
        public bool RequireTotalCount { get => el.GetProperty("requireTotalCount").GetBoolean(); }
        public int Skip { get => el.GetProperty("skip").GetInt32(); }
        public int Take { get => el.GetProperty("take").GetInt32(); }
        /// <summary>
        /// Означает что было открыто окно фильтрования. Если пользователь не нажал и ничего не ввел в поиск, то больше фильтров не будет. Иначе - стандартная структура поиска.
        /// Ответ надо отправлять как обычно. если будет отправлено 10 объектов, то в поиске будет только 10 и никакой прокрутки. даже если указать totalCount
        /// При этом при больших списках каждая догрузка отправляет запрос (например '{"userData":{},"dataField":"Id"}') без дополнительных данных
        /// </summary>
        public string? DataField { get => el.TryGetProperty("dataField", out var val) ? val.GetString() : null; }
        /// <summary>
        /// https://js.devexpress.com/React/Documentation/23_2/ApiReference/Data_Layer/DataSource/Configuration/#filter
        /// </summary>
        public Filters Filter { get => new Filters(el.GetProperty("filter")); }
        // '[{"selector":"valueAsString","desc":false}]'
        public Sorts Sort { get => new Sorts(el.GetProperty("sort")); }

        public PagingRequest(JsonElement el)
        {
            this.el = el;
        }

        public struct Sorts
        {
            public readonly JsonElement jsonElement;
            public Sorts(JsonElement jsonElement)
            {
                this.jsonElement = jsonElement;
            }
        }

        public struct Filters
        {
            public readonly JsonElement jsonElement;
            public Filters(JsonElement jsonElement)
            {
                this.jsonElement = jsonElement;
            }
        }
    }

    public class PagingResponse<T>
    {
        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }
        [JsonPropertyName("data")]
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
    }
}