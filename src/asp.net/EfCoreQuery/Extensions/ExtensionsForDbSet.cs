// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using System.Text;
using Microsoft.EntityFrameworkCore;

namespace EfCoreQuery.Controllers
{
    public static class ExtensionsForDbSet
    {
        /*
         * При больщом списке
Hash Join  (cost=2.75..248.25 rows=98 width=32) (actual time=0.052..1.334 rows=100 loops=1)
  Hash Cond: ((source.id_part1 = "*VALUES*".column1) AND (source.id_part2 = "*VALUES*".column2))
  ->  Seq Scan on composite_key_joins source  (cost=0.00..171.01 rows=9801 width=32) (actual time=0.003..0.454 rows=9801 loops=1)
  ->  Hash  (cost=1.25..1.25 rows=100 width=8) (actual time=0.036..0.037 rows=100 loops=1)
        Buckets: 1024  Batches: 1  Memory Usage: 12kB
        ->  Values Scan on "*VALUES*"  (cost=0.00..1.25 rows=100 width=8) (actual time=0.002..0.019 rows=100 loops=1)
Planning Time: 0.283 ms
Execution Time: 1.356 ms


SELECT source.* FROM composite_key_joins source
JOIN (VALUES (1,1),(1,2),(1,3),(1,4),(1,5),(1,6),(1,7),(1,8),(1,9),(1,10),(1,11),(1,12),(1,13),(1,14),(1,15),(1,16),(1,17),(1,18),(1,19),(1,20),(1,21),(1,22),(1,23),(1,24),(1,25),(1,26),(1,27),(1,28),(1,29),(1,30),(1,31),(1,32),(1,33),(1,34),(1,35),(1,36),(1,37),(1,38),(1,39),(1,40),(1,41),(1,42),(1,43),(1,44),(1,45),(1,46),(1,47),(1,48),(1,49),(1,50),(1,51),(1,52),(1,53),(1,54),(1,55),(1,56),(1,57),(1,58),(1,59),(1,60),(1,61),(1,62),(1,63),(1,64),(1,65),(1,66),(1,67),(1,68),(1,69),(1,70),(1,71),(1,72),(1,73),(1,74),(1,75),(1,76),(1,77),(1,78),(1,79),(1,80),(1,81),(1,82),(1,83),(1,84),(1,85),(1,86),(1,87),(1,88),(1,89),(1,90),(1,91),(1,92),(1,93),(1,94),(1,95),(1,96),(1,97),(1,98),(1,99),(2,1)) AS temp(pk1, pk2) ON source.id_part1=temp.pk1 AND source.id_part2=temp.pk2
         */
        public static IQueryable<T> JoinByCompositeKey<T, TPK1, TPK2>(this DbSet<T> dbset,
                                                                      IEnumerable<(TPK1, TPK2)> keys,
                                                                      string tableName,
                                                                      string nameOfPK1,
                                                                      string nameOfPK2) where T : class
        {
            if (!keys.Any()) throw new ArgumentException();
            var sb = new StringBuilder();
            var values = string.Empty;
            foreach (var key in keys)
            {
                sb.Append('(');
                sb.Append(key.Item1.ToString());
                sb.Append(',');
                sb.Append(key.Item2.ToString());
                sb.Append(')');
                sb.Append(',');
            }
            sb.Length = sb.Length - 1;

            values = sb.ToString();
            var q = dbset.FromSqlRaw(
                $@"SELECT source.* FROM {tableName} source 
                JOIN (VALUES {values}) AS temp(pk1, pk2) ON source.{nameOfPK1}=temp.pk1 AND source.{nameOfPK2}=temp.pk2");
            return q;
        }


        /*
Hash Join  (cost=2.75..248.25 rows=98 width=32) (actual time=0.041..0.991 rows=100 loops=1)
  Hash Cond: ((source.id_part1 = "*VALUES*".column1) AND (source.id_part2 = "*VALUES*".column2))
  ->  Seq Scan on composite_key_joins source  (cost=0.00..171.01 rows=9801 width=32) (actual time=0.004..0.336 rows=9801 loops=1)
  ->  Hash  (cost=1.25..1.25 rows=100 width=8) (actual time=0.027..0.028 rows=100 loops=1)
        Buckets: 1024  Batches: 1  Memory Usage: 12kB
        ->  Values Scan on "*VALUES*"  (cost=0.00..1.25 rows=100 width=8) (actual time=0.001..0.015 rows=100 loops=1)
Planning Time: 0.202 ms
Execution Time: 1.035 ms
         */
        public static IQueryable<T> JoinByCompositeKeyV1<T, TPK1, TPK2>(this DbSet<T> dbset,
                                                                      IEnumerable<(TPK1, TPK2)> keys,
                                                                      string tableName,
                                                                      string nameOfPK1,
                                                                      string nameOfPK2) where T : class
        {
            if (!keys.Any()) throw new ArgumentException();
            var sb = new StringBuilder();
            var values = string.Empty;
            foreach (var key in keys)
            {
                sb.Append('(');
                sb.Append(key.Item1.ToString());
                sb.Append(',');
                sb.Append(key.Item2.ToString());
                sb.Append(')');
                sb.Append(',');
            }
            sb.Length = sb.Length - 1;

            values = sb.ToString();
            var q = dbset.FromSqlRaw(
                $@"SELECT source.* FROM {tableName} source 
                JOIN (VALUES {values}) AS temp(pk1, pk2) ON (source.{nameOfPK1}, source.{nameOfPK2})=(temp.pk1, temp.pk2)");
            return q;
        }


        public static IQueryable<T> JoinByCompositeKeyV5<T, TPK1, TPK2>(this DbSet<T> dbset,
                                                                   IEnumerable<(TPK1, TPK2)> keys,
                                                                   string tableName,
                                                                   string nameOfPK1,
                                                                   string nameOfPK2) where T : class
        {
            if (!keys.Any()) throw new ArgumentException();
            var sb = new StringBuilder();
            var values = string.Empty;
            foreach (var key in keys)
            {
                sb.Append('(');
                sb.Append(key.Item1.ToString());
                sb.Append(',');
                sb.Append(key.Item2.ToString());
                sb.Append(')');
                sb.Append(',');
            }
            sb.Length = sb.Length - 1;

            values = sb.ToString();
            var q = dbset.FromSqlRaw(
                $@"SELECT source.* FROM {tableName} source 
               RIGHT JOIN (VALUES {values}) AS temp(pk1, pk2) ON (source.{nameOfPK1}, source.{nameOfPK2})=(temp.pk1, temp.pk2)");
            return q;
        }

        /*
        explain analyze      
        SELECT source.* FROM composite_key_joins source                    
        WHERE (source.id_part1, source.id_part2) = ANY (ARRAY[(1,1),(1,2),(1,3),(1,4),(1,5),(1,6),(1,7),(1,8),(1,9),(1,10),(1,11),(1,12),(1,13),(1,14),(1,15),(1,16),(1,17),(1,18),(1,19),(1,20),(1,21),(1,22),(1,23),(1,24),(1,25),(1,26),(1,27),(1,28),(1,29),(1,30),(1,31),(1,32),(1,33),(1,34),(1,35),(1,36),(1,37),(1,38),(1,39),(1,40),(1,41),(1,42),(1,43),(1,44),(1,45),(1,46),(1,47),(1,48),(1,49),(1,50),(1,51),(1,52),(1,53),(1,54),(1,55),(1,56),(1,57),(1,58),(1,59),(1,60),(1,61),(1,62),(1,63),(1,64),(1,65),(1,66),(1,67),(1,68),(1,69),(1,70),(1,71),(1,72),(1,73),(1,74),(1,75),(1,76),(1,77),(1,78),(1,79),(1,80),(1,81),(1,82),(1,83),(1,84),(1,85),(1,86),(1,87),(1,88),(1,89),(1,90),(1,91),(1,92),(1,93),(1,94),(1,95),(1,96),(1,97),(1,98),(1,99),(2,1)])
         
        Seq Scan on composite_key_joins source  (cost=0.25..220.26 rows=4901 width=32) (actual time=0.065..1.648 rows=100 loops=1)
        Filter: (ROW(id_part1, id_part2) = ANY ('{"(1,1)","(1,2)","(1,3)","(1,4)","(1,5)","(1,6)","(1,7)","(1,8)","(1,9)","(1,10)","(1,11)","(1,12)","(1,13)","(1,14)","(1,15)","(1,16)","(1,17)","(1,18)","(1,19)","(1,20)","(1,21)","(1,22)","(1,23)","(1,24)","(1,25)","(1,26)","(1,27)","(1,28)","(1,29)","(1,30)","(1,31)","(1,32)","(1,33)","(1,34)","(1,35)","(1,36)","(1,37)","(1,38)","(1,39)","(1,40)","(1,41)","(1,42)","(1,43)","(1,44)","(1,45)","(1,46)","(1,47)","(1,48)","(1,49)","(1,50)","(1,51)","(1,52)","(1,53)","(1,54)","(1,55)","(1,56)","(1,57)","(1,58)","(1,59)","(1,60)","(1,61)","(1,62)","(1,63)","(1,64)","(1,65)","(1,66)","(1,67)","(1,68)","(1,69)","(1,70)","(1,71)","(1,72)","(1,73)","(1,74)","(1,75)","(1,76)","(1,77)","(1,78)","(1,79)","(1,80)","(1,81)","(1,82)","(1,83)","(1,84)","(1,85)","(1,86)","(1,87)","(1,88)","(1,89)","(1,90)","(1,91)","(1,92)","(1,93)","(1,94)","(1,95)","(1,96)","(1,97)","(1,98)","(1,99)","(2,1)"}'::record[]))
        Rows Removed by Filter: 9701
        Planning Time: 0.215 ms
        Execution Time: 1.655 ms
         */
        public static IQueryable<T> JoinByCompositeKeyV2<T, TPK1, TPK2>(this DbSet<T> dbset,
                                                                      IEnumerable<(TPK1, TPK2)> keys,
                                                                      string tableName,
                                                                      string nameOfPK1,
                                                                      string nameOfPK2) where T : class
        {
            if (!keys.Any()) throw new ArgumentException();
            var sb = new StringBuilder();
            var values = string.Empty;
            foreach (var key in keys)
            {
                sb.Append('(');
                sb.Append(key.Item1.ToString());
                sb.Append(',');
                sb.Append(key.Item2.ToString());
                sb.Append(')');
                sb.Append(',');
            }
            sb.Length = sb.Length - 1;

            values = sb.ToString();
            var q = dbset.FromSqlRaw(
                $@"SELECT source.* FROM {tableName} source 
                WHERE (source.{nameOfPK1}, source.{nameOfPK2}) = ANY (ARRAY[{values}])");
            return q;
        }

        public static IQueryable<T> JoinByCompositeKeyV3<T, TPK1, TPK2>(this DbSet<T> dbset,
                                                                     IEnumerable<(TPK1, TPK2)> keys,
                                                                     string tableName,
                                                                     string nameOfPK1,
                                                                     string nameOfPK2) where T : class
        {
            if (!keys.Any()) throw new ArgumentException();
            var sb = new StringBuilder();
            var values = string.Empty;
            foreach (var key in keys)
            {
                sb.Append("ROW(");
                sb.Append(key.Item1.ToString());
                sb.Append(',');
                sb.Append(key.Item2.ToString());
                sb.Append(')');
                sb.Append(',');
            }
            sb.Length = sb.Length - 1;

            values = sb.ToString();
            var q = dbset.FromSqlRaw(
                $@"SELECT source.* FROM {tableName} source 
                JOIN unnest(ARRAY[{values}]) AS keys(pk1, pk2) ON source.{nameOfPK1}=keys.pk1 AND source.{nameOfPK2}=keys.pk2");
            return q;
        }



        public static IQueryable<T> JoinByCompositeKeyUsingIndex<T, TPK1, TPK2>(this DbSet<T> dbset,
                                                                     IEnumerable<(TPK1, TPK2)> keys,
                                                                     string tableName,
                                                                     string nameOfPK1,
                                                                     string nameOfPK2) where T : class
        {
            if (!keys.Any()) throw new ArgumentException();
            var sb = new StringBuilder();
            var values = string.Empty;
            foreach (var key in keys)
            {
                sb.Append("(");
                sb.Append(key.Item1.ToString());
                sb.Append(',');
                sb.Append(key.Item2.ToString());
                sb.Append(')');
                sb.Append(',');
            }
            sb.Length = sb.Length - 1;

            values = sb.ToString();
            var q = dbset.FromSqlRaw(
                $@"SET enable_seqscan TO off; SET enable_indexscan = on; SET enable_hashjoin = off; 

                            SELECT t2.* FROM (VALUES {values}) AS temp(pk1, pk2) 
                            JOIN (SELECT source.* FROM {tableName} source) as t2
                            ON t2.{nameOfPK1}=temp.pk1 AND t2.{nameOfPK2}=temp.pk2
            ");

            //            var q = dbset.FromSqlRaw(
            //                $@"SELECT t2.* FROM (VALUES {values}) AS temp(pk1, pk2) 
            //                JOIN (SELECT source.* FROM {tableName} source) as t2
            //                ON t2.{nameOfPK1}=temp.pk1 AND t2.{nameOfPK2}=temp.pk2
            //");
            return q;
        }
    }
}
