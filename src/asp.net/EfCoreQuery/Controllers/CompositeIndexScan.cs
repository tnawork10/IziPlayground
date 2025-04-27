using System.Text;
using System.Text.Json;
using IziHardGames.Playgrounds.ForEfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EfCoreQuery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompositeIndexScan(CompositeIndexScanDbContext context) : ControllerBase
    {
  

        [HttpPost(nameof(GetAny10))]
        public async Task<IActionResult> GetAny10()
        {
            return Ok(await context.EntityiesWithCompositeIndex.OrderBy(x => x.GuidFirst).Take(10).ToArrayAsync());
        }

        [HttpPost(nameof(Find10))]
        public async Task<IActionResult> Find10()
        {
            string json = @"[
  {
    ""id"": 1654,
    ""guidFirst"": ""001726c3-688d-4de6-b98c-a943928a4447"",
    ""guidSecond"": ""5ce47512-c5ec-403c-81ef-6bd7a343b66b"",
    ""type"": 2
  },
  {
    ""id"": 456,
    ""guidFirst"": ""0022d527-61d1-4779-8eb3-731f84520323"",
    ""guidSecond"": ""500e8011-7b33-4137-99e3-bdc4927f06e5"",
    ""type"": 0
  },
  {
    ""id"": 1354,
    ""guidFirst"": ""003be889-dc8f-4067-935b-d237d6870d92"",
    ""guidSecond"": ""90726f7f-3e1a-4e76-9063-ffd8908133f3"",
    ""type"": 2
  },
  {
    ""id"": 845,
    ""guidFirst"": ""00419f49-d507-4122-adf7-d70f9d900226"",
    ""guidSecond"": ""9fe2d3d1-6016-4624-b4c9-7601349b4058"",
    ""type"": 1
  },
  {
    ""id"": 390,
    ""guidFirst"": ""009c0073-3ec2-42ca-a856-c355afda89f2"",
    ""guidSecond"": ""4b6846a0-12fc-4a3b-8329-84b48211c1e7"",
    ""type"": 2
  },
  {
    ""id"": 1495,
    ""guidFirst"": ""00a65abf-c926-4fe1-8782-bee7dad42ff1"",
    ""guidSecond"": ""ef8e9a37-29b1-4305-bf5d-193423bc6515"",
    ""type"": 3
  },
  {
    ""id"": 1127,
    ""guidFirst"": ""00a86c86-dbfd-41c2-9351-22b6edb72fe9"",
    ""guidSecond"": ""49f5d10a-89bb-4c15-89f3-192b2c85abee"",
    ""type"": 3
  },
  {
    ""id"": 946,
    ""guidFirst"": ""00acb7f6-f1af-4587-a4e4-667c46c4f466"",
    ""guidSecond"": ""83cbbd6a-802b-4ed0-8653-ec3aedbfc2dc"",
    ""type"": 2
  },
  {
    ""id"": 796,
    ""guidFirst"": ""00ae2ee7-e0c4-4d89-b749-2f95393c12c4"",
    ""guidSecond"": ""ee4ea598-c2ab-47cf-a089-445e50eb3f8b"",
    ""type"": 0
  },
  {
    ""id"": 1851,
    ""guidFirst"": ""00b29878-8043-41e5-93fd-652536ee221a"",
    ""guidSecond"": ""f2b34f9c-4bce-4d26-be0b-fde0fc867452"",
    ""type"": 3
  }
]";

            var es = JsonSerializer.Deserialize<EntityCompositeIndex[]>(json, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });


            var sb = new StringBuilder();
            var values = string.Empty;
            foreach (var item in es)
            {
                sb.Append("(");
                sb.Append((int)item.Type);
                sb.Append(',');
                sb.Append($"'{item.GuidFirst.ToString()}'");
                sb.Append(',');
                sb.Append($"'{item.GuidSecond.ToString()}'");
                sb.Append(')');
                sb.Append(',');
            }
            sb.Length = sb.Length - 1;

            values = sb.ToString();
            var tableName = "entityies_with_composite_index";
            var cName1 = "type";
            var cName2 = "guid_first";
            var cName3 = "guid_second";

            var s = $@"SET enable_seqscan TO off; SET enable_indexscan = on; SET enable_hashjoin = off; 

                            SELECT t2.* FROM (VALUES {values}) AS temp(pk1, pk2, pk3) 
                            JOIN (SELECT source.* FROM {tableName} source) as t2
                            ON t2.{cName1}=temp.pk1 AND t2.{cName2}=temp.pk2::UUID AND t2.{cName3}=temp.pk3::UUID";

            var q = context.EntityiesWithCompositeIndex.FromSqlRaw(s);

            //            var q = dbset.FromSqlRaw(
            //                $@"SELECT t2.* FROM (VALUES {values}) AS temp(pk1, pk2) 
            //                JOIN (SELECT source.* FROM {tableName} source) as t2
            //                ON t2.{nameOfPK1}=temp.pk1 AND t2.{nameOfPK2}=temp.pk2
            //");
            try
            {
                var res = await q.ToArrayAsync();
                return Ok(new { Qstring = s, Res = res });

            }
            catch (Exception ex)
            {
                return BadRequest(new { Qstring = s, Ex = ex.Message });
            }
        }

    }
}
