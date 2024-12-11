using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IziHardGames.Playgrounds.ForEfCore
{
    public class CheckDbCommands
    {
        /// <include file='C:\Users\ivan\Documents\.csharp\IziPlayGround\src\asp.net\EfCoreQuery\bin\Debug\net8.0\EfCoreQuery.xml' path=""'/>
        public static async Task Check()
        {
            // var json = "[\n  {\n    \"hardwareId\": 0,\n    \"signalId\": 0\n  }\n]";

            // if(Enumerable.Range(0, 2).Contains(2))
            //  {
            //         Console.WriteLine("Fire");
            //  }
            // Console.WriteLine("Hello, World!");
            // JsonArray obj = JsonObject.Parse(json).AsArray();

            // foreach (var VARIABLE in obj)
            // {
            //     Console.WriteLine(VARIABLE["hardwareId"].GetValue<long>());
            //     Console.WriteLine(VARIABLE["signalId"].GetValue<long>());
            // }
            using var v = new PlaygroundDbContext();
            //await v.Database.EnsureDeletedAsync();
            //await v.Database.EnsureCreatedAsync();

            //for (int i = 0; i < 1000; i++)
            //{
            //    v.Models0.Add(PlaygroundDbContext.CreateModel0());
            //}
            //await v.SaveChangesAsync();
            //var a = v.Models0.Include(x => x.Model1).ThenInclude(x => x.Model2s).ThenInclude(x => x.ModelA).First();\

            if (false)
            {
                var models01 = v.Models0.Include(x => x.Model1s).First(x => x.Id == 5).Model1s.ToArray();
                /*
                    EXPLAIN
                    SELECT t."Id", t."Guid", t."Model1Id", t."String", t."UniqString", m0."Id", m0."Model0Id", m0."ValueAsLong"
                    FROM (
                      SELECT m."Id", m."Guid", m."Model1Id", m."String", m."UniqString"
                      FROM "Models0" AS m
                      WHERE m."Id" = 5
                      LIMIT 1
                    ) AS t
                    LEFT JOIN "Models1" AS m0 ON t."Id" = m0."Model0Id"
                    ORDER BY t."Id"
                 =========================
                    "QUERY PLAN"
                    "Sort  (cost=16.77..16.78 rows=5 width=78)"
                    "  Sort Key: m.""Id"""
                    "  ->  Nested Loop Left Join  (cost=0.56..16.71 rows=5 width=78)"
                    "        ->  Limit  (cost=0.28..8.29 rows=1 width=62)"
                    "              ->  Index Scan using ""PK_Models0"" on ""Models0"" m  (cost=0.28..8.29 rows=1 width=62)"
                    "                    Index Cond: (""Id"" = 5)"
                    "        ->  Index Scan using ""IX_Models1_Model0Id"" on ""Models1"" m0  (cost=0.28..8.37 rows=5 width=16)"
                    "              Index Cond: (""Model0Id"" = m.""Id"")"


                 */
            }

            if (false)
            {
                var models01 = v.Models0.Include(x => x.Model1s).First(x => x.Id == 5).Model1s.Select(x => x).ToArray();

                /*
                    SELECT t."Id", t."Guid", t."Model1Id", t."String", t."UniqString", m0."Id", m0."Model0Id", m0."ValueAsLong"
                    FROM (
                      SELECT m."Id", m."Guid", m."Model1Id", m."String", m."UniqString"
                      FROM "Models0" AS m
                      WHERE m."Id" = 5
                      LIMIT 1
                    ) AS t
                    LEFT JOIN "Models1" AS m0 ON t."Id" = m0."Model0Id"
                    ORDER BY t."Id"
                ===========================
                    "QUERY PLAN"
                    "Sort  (cost=16.77..16.78 rows=5 width=78)"
                    "  Sort Key: m.""Id"""
                    "  ->  Nested Loop Left Join  (cost=0.56..16.71 rows=5 width=78)"
                    "        ->  Limit  (cost=0.28..8.29 rows=1 width=62)"
                    "              ->  Index Scan using ""PK_Models0"" on ""Models0"" m  (cost=0.28..8.29 rows=1 width=62)"
                    "                    Index Cond: (""Id"" = 5)"
                    "        ->  Index Scan using ""IX_Models1_Model0Id"" on ""Models1"" m0  (cost=0.28..8.37 rows=5 width=16)"
                    "              Index Cond: (""Model0Id"" = m.""Id"")"

                 */
            }





            if (false)
            {
                var q0 = v.Models0.Find(5).Model1s.ToArray();   // null reference exception
            }

            if (false)
            {
                var q1 = v.Models0.Where(x => x.Id == 5).Select(x => x.Model1s).ToArray();
                /*
                    SELECT m."Id", m0."Id", m0."Model0Id", m0."ValueAsLong"
                    FROM "Models0" AS m
                    LEFT JOIN "Models1" AS m0 ON m."Id" = m0."Model0Id"
                    WHERE m."Id" = 5
                    ORDER BY m."Id"
                ======================================================
                    "QUERY PLAN"
                    "Nested Loop Left Join  (cost=0.56..16.71 rows=5 width=20)"
                    "  ->  Index Only Scan using ""PK_Models0"" on ""Models0"" m  (cost=0.28..8.29 rows=1 width=4)"
                    "        Index Cond: (""Id"" = 5)"
                    "  ->  Index Scan using ""IX_Models1_Model0Id"" on ""Models1"" m0  (cost=0.28..8.37 rows=5 width=16)"
                    "        Index Cond: (""Model0Id"" = 5)"
                 */
            }

            if (false)
            {
                var q1 = v.Models0.Include(x => x.Model1s).Where(x => x.Id == 5).Select(x => x.Model1s).ToArray();

                /*
                    SELECT m."Id", m0."Id", m0."Model0Id", m0."ValueAsLong"
                    FROM "Models0" AS m
                    LEFT JOIN "Models1" AS m0 ON m."Id" = m0."Model0Id"
                    WHERE m."Id" = 5
                    ORDER BY m."Id"

                    ========================
                    "QUERY PLAN"
                    "Nested Loop Left Join  (cost=0.56..16.71 rows=5 width=20)"
                    "  ->  Index Only Scan using ""PK_Models0"" on ""Models0"" m  (cost=0.28..8.29 rows=1 width=4)"
                    "        Index Cond: (""Id"" = 5)"
                    "  ->  Index Scan using ""IX_Models1_Model0Id"" on ""Models1"" m0  (cost=0.28..8.37 rows=5 width=16)"
                    "        Index Cond: (""Model0Id"" = 5)"

                */

            }

            if (false)
            {
                var q4 = v.Models0.Where(x => x.Id == 5).Select(x => x.Model1s).SelectMany(y => y.Select(x => x.Model2s)).ToArray();
                /*
                    SELECT m."Id", m0."Id", m1."Id", m1."Model1Id", m1."ModelAId", m1."ModelBId"
                    FROM "Models0" AS m
                    INNER JOIN "Models1" AS m0 ON m."Id" = m0."Model0Id"
                    LEFT JOIN "Models2" AS m1 ON m0."Id" = m1."Model1Id"
                    WHERE m."Id" = 5
                    ORDER BY m."Id", m0."Id"     
                =========================================
                    "QUERY PLAN"
                    "Sort  (cost=59.42..59.48 rows=25 width=24)"
                    "  Sort Key: m0.""Id"""
                    "  ->  Nested Loop Left Join  (cost=0.84..58.84 rows=25 width=24)"
                    "        ->  Nested Loop  (cost=0.56..16.71 rows=5 width=8)"
                    "              ->  Index Only Scan using ""PK_Models0"" on ""Models0"" m  (cost=0.28..8.29 rows=1 width=4)"
                    "                    Index Cond: (""Id"" = 5)"
                    "              ->  Index Scan using ""IX_Models1_Model0Id"" on ""Models1"" m0  (cost=0.28..8.37 rows=5 width=8)"
                    "                    Index Cond: (""Model0Id"" = 5)"
                    "        ->  Index Scan using ""IX_Models2_Model1Id"" on ""Models2"" m1  (cost=0.29..8.38 rows=5 width=16)"
                    "              Index Cond: (""Model1Id"" = m0.""Id"")"

                 */
            }


            if (false)
            {
                var q5 = v.Models0.Where(x => x.Id == 5).SelectMany(x => x.Model1s).Select(x => x.Model2s).ToArray();
                /*
                    SELECT m."Id", m0."Id", m1."Id", m1."Model1Id", m1."ModelAId", m1."ModelBId"
                    FROM "Models0" AS m
                    INNER JOIN "Models1" AS m0 ON m."Id" = m0."Model0Id"
                    LEFT JOIN "Models2" AS m1 ON m0."Id" = m1."Model1Id"
                    WHERE m."Id" = 5
                    ORDER BY m."Id", m0."Id"
                =======================================================
                    "QUERY PLAN"
                    "Sort  (cost=59.42..59.48 rows=25 width=24)"
                    "  Sort Key: m0.""Id"""
                    "  ->  Nested Loop Left Join  (cost=0.84..58.84 rows=25 width=24)"
                    "        ->  Nested Loop  (cost=0.56..16.71 rows=5 width=8)"
                    "              ->  Index Only Scan using ""PK_Models0"" on ""Models0"" m  (cost=0.28..8.29 rows=1 width=4)"
                    "                    Index Cond: (""Id"" = 5)"
                    "              ->  Index Scan using ""IX_Models1_Model0Id"" on ""Models1"" m0  (cost=0.28..8.37 rows=5 width=8)"
                    "                    Index Cond: (""Model0Id"" = 5)"
                    "        ->  Index Scan using ""IX_Models2_Model1Id"" on ""Models2"" m1  (cost=0.29..8.38 rows=5 width=16)"
                    "              Index Cond: (""Model1Id"" = m0.""Id"")"

                 */
            }

            if (false)
            {
                var q7 = v.Models0.Where(x => x.Id == 5).SelectMany(x => x.Model1s).Select(x => x.Model2s).AsSplitQuery().ToArray();
                /*
                   SELECT m."Id", m0."Id"
                   FROM "Models0" AS m
                   INNER JOIN "Models1" AS m0 ON m."Id" = m0."Model0Id"
                   WHERE m."Id" = 5
                   ORDER BY m."Id", m0."Id"
                ++++++++++++++++++++++++++++++++++++++++
                    SELECT m1."Id", m1."Model1Id", m1."ModelAId", m1."ModelBId", m."Id", m0."Id"
                    FROM "Models0" AS m
                    INNER JOIN "Models1" AS m0 ON m."Id" = m0."Model0Id"
                    INNER JOIN "Models2" AS m1 ON m0."Id" = m1."Model1Id"
                    WHERE m."Id" = 5
                    ORDER BY m."Id", m0."Id"
                ============================
                "QUERY PLAN"
                "Sort  (cost=16.77..16.78 rows=5 width=8)"
                "  Sort Key: m0.""Id"""
                "  ->  Nested Loop  (cost=0.56..16.71 rows=5 width=8)"
                "        ->  Index Only Scan using ""PK_Models0"" on ""Models0"" m  (cost=0.28..8.29 rows=1 width=4)"
                "              Index Cond: (""Id"" = 5)"
                "        ->  Index Scan using ""IX_Models1_Model0Id"" on ""Models1"" m0  (cost=0.28..8.37 rows=5 width=8)"
                "              Index Cond: (""Model0Id"" = 5)"
                ++++++++++++++++++++++++++++++++++++++++
                "QUERY PLAN"
                "Sort  (cost=59.42..59.48 rows=25 width=24)"
                "  Sort Key: m1.""Model1Id"""
                "  ->  Nested Loop  (cost=0.84..58.84 rows=25 width=24)"
                "        ->  Nested Loop  (cost=0.56..16.71 rows=5 width=8)"
                "              ->  Index Only Scan using ""PK_Models0"" on ""Models0"" m  (cost=0.28..8.29 rows=1 width=4)"
                "                    Index Cond: (""Id"" = 5)"
                "              ->  Index Scan using ""IX_Models1_Model0Id"" on ""Models1"" m0  (cost=0.28..8.37 rows=5 width=8)"
                "                    Index Cond: (""Model0Id"" = 5)"
                "        ->  Index Scan using ""IX_Models2_Model1Id"" on ""Models2"" m1  (cost=0.29..8.38 rows=5 width=16)"
                "              Index Cond: (""Model1Id"" = m0.""Id"")"
                 */
            }
            if (false)
            {
                var q6 = v.Models2.Where(x => x.Model1.Model0.Id == 5).ToArray();
                /*
                    SELECT m."Id", m."Model1Id", m."ModelAId", m."ModelBId"
                    FROM "Models2" AS m
                    INNER JOIN "Models1" AS m0 ON m."Model1Id" = m0."Id"
                    INNER JOIN "Models0" AS m1 ON m0."Model0Id" = m1."Id"
                    WHERE m1."Id" = 5
                ===================================================
                    "QUERY PLAN"
                    "Nested Loop  (cost=0.84..58.84 rows=25 width=16)"
                    "  ->  Nested Loop  (cost=0.56..16.71 rows=5 width=4)"
                    "        ->  Index Only Scan using ""PK_Models0"" on ""Models0"" m1  (cost=0.28..8.29 rows=1 width=4)"
                    "              Index Cond: (""Id"" = 5)"
                    "        ->  Index Scan using ""IX_Models1_Model0Id"" on ""Models1"" m0  (cost=0.28..8.37 rows=5 width=8)"
                    "              Index Cond: (""Model0Id"" = 5)"
                    "  ->  Index Scan using ""IX_Models2_Model1Id"" on ""Models2"" m  (cost=0.29..8.38 rows=5 width=16)"
                    "        Index Cond: (""Model1Id"" = m0.""Id"")"

                */
            }


            //var models10 = models01.Model1s.ToArray();


            //Console.WriteLine("============================");

            //var models11 = v.Models1.Where(x => x.Model0.Id == 5).ToArray();

            //var models02 = v.Models2.Where(x=>x.).First(x => x.Id == 5);

            //(x=>x.modelBs).ToArray();
            //var modelsB = a.ModelBs.ToArray();

            //for (int i = 0; i < modelsB.Length; i++)
            //{
            //    Console.WriteLine($"{modelsB[i].Id}; {modelsB[i].Name}");
            //}


            if (false)
            {
                var strs = new string[] { "1", "3", "5", "7" }.ToList();
                var result = v.ModelsB.Where(x => strs.Contains(x.KeyCounter)).ToArray();

                foreach (var item in result)
                {
                    Console.WriteLine($"{item.Id}; {item.KeyCounter}");
                }

                //Executed DbCommand(58ms)
                //[Parameters= [@__strs_0 ={ '1', '3', '5', '7' } (DbType = Object)], CommandType = 'Text', CommandTimeout = '30']
                //SELECT m."Id", m."KeyCounter", m."Name"
                //FROM "ModelsB" AS m
                //WHERE m."KeyCounter" = ANY(@__strs_0)
                //===================================================

            }

            if (true)
            {
                var strs = new string[] { "1", "3", "5", "7" }.ToList();
                var result = v.ModelsB.Join(strs, x => x.KeyCounter, y => y, (x, y) => x).ToArray();

                foreach (var item in result)
                {
                    Console.WriteLine($"{item.Id}; {item.KeyCounter}");
                }

                //Executed DbCommand(55ms)
                //[Parameters= [@__p_0 ={ '1', '3', '5', '7' } (DbType = Object)], CommandType = 'Text', CommandTimeout = '30']
                //SELECT m."Id", m."KeyCounter", m."Name"
                //FROM "ModelsB" AS m
                //INNER JOIN unnest(@__p_0) AS p(value) ON m."KeyCounter" = p.value
                //===================================================

            }

            Console.WriteLine("Complete");
            Console.ReadLine();

        }
    }
}
