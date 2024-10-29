using System.Data;
using System.Linq.Expressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EfCoreQuery.Controllers
{
    public static class ExtensionsForIQueryable
    {
        /// <summary>
        /// Метод разворачивается в where (item[N].TId1 == x.TId1 || where item[N].TId2 == x.TId2) и так на каждый элемент ids
        /// 
        /*
        Bitmap Heap Scan on "CompositeKeyJoins" c  (cost=12.89..23.14 rows=3 width=32)
          Recheck Cond: ((("IdPart1" = 6) AND ("IdPart2" = 14)) OR (("IdPart1" = 23) AND ("IdPart2" = 9)) OR (("IdPart1" = 94) AND ("IdPart2" = 73)))
          ->  BitmapOr  (cost=12.89..12.89 rows=3 width=0)
        ->  Bitmap Index Scan on "PK_CompositeKeyJoins"  (cost=0.00..4.29 rows=1 width=0)
              Index Cond: (("IdPart1" = 6) AND ("IdPart2" = 14))
        ->  Bitmap Index Scan on "PK_CompositeKeyJoins"  (cost=0.00..4.29 rows=1 width=0)
              Index Cond: (("IdPart1" = 23) AND ("IdPart2" = 9))
        ->  Bitmap Index Scan on "PK_CompositeKeyJoins"  (cost=0.00..4.29 rows=1 width=0)
              Index Cond: (("IdPart1" = 94) AND ("IdPart2" = 73))
        */
        /// </summary>
        /// <remarks>Много боксинга, но зато поиск по ключам (не Scan Index, а Bitmap Index Scan)</remarks>
        public static IQueryable<T> CompositeKeyFilterP2<T, TId1, TId2>(this IQueryable<T> queriable, IEnumerable<(TId1, TId2)> ids, string propName0, string propName1)

        {
            var parameter = Expression.Parameter(typeof(T));

            var body = ids.Select(b => Expression.AndAlso(
                Expression.Equal(Expression.Property(parameter, propName0),
                                 Expression.Constant(b.Item1)),
                Expression.Equal(Expression.Property(parameter, propName1),
                             Expression.Constant(b.Item2))))
            .Aggregate(Expression.OrElse);

            var predicate = Expression.Lambda<Func<T, bool>>(body, parameter);
            return queriable.Where(predicate);
        }
        public static IQueryable<T> CompositeKeyFilterP3<T, TId1, TId2, TId3>(this IQueryable<T> queriable, IEnumerable<(TId1, TId2, TId3)> ids, string propName0, string propName1, string propName3)

        {
            var parameter = Expression.Parameter(typeof(T));

            var body = ids.Select(b => Expression.AndAlso(Expression.AndAlso(
                Expression.Equal(Expression.Property(parameter, propName0),
                                 Expression.Constant(b.Item1)),
                Expression.Equal(Expression.Property(parameter, propName1),
                                Expression.Constant(b.Item2))
                ),
                 Expression.Equal(Expression.Property(parameter, propName3),
                                Expression.Constant(b.Item3))
                ))
            .Aggregate(Expression.OrElse);

            var predicate = Expression.Lambda<Func<T, bool>>(body, parameter);
            return queriable.Where(predicate);
        }
    }
}
