#pragma warning disable
using System.Linq.Expressions;
using IziHardGames.Playgrounds.ForEfCore;
using Microsoft.EntityFrameworkCore;

namespace EfCoreQuery
{
    public class QueryBackgroundService(IServiceProvider provider) : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return;
            using var scope = provider.CreateScope();
            //var context = scope.ServiceProvider.GetRequiredService<QueryDbContext>();
            var context = scope.ServiceProvider.GetRequiredService<QueryDbContext>();
            var outer = await context.CompositeKeyJoins.Take(5).ToArrayAsync();
            var tuples = outer.Select(x => (x.IdPart1, x.IdPart2)).ToList();
            var expre = GenerateContainsExpression(tuples);
            var func = expre.Compile();
            var q = context.CompositeKeyJoins.Where(expre);
            var qString = q.ToQueryString();
            var res = await q.ToArrayAsync();
            if (false)
            {
                var q2 = context.CompositeKeyJoins.Where(x => func(x));
                // exception
                var res2 = await q2.ToArrayAsync();
            }
        }


        public static Expression<Func<CompositeKeyJoin, bool>> GenerateContainsExpression(List<(int, int)> tupleCollection)
        {
            /*
            SELECT c.id_part1, c.id_part2, c.some_random_value, c.value
            FROM composite_key_joins AS c
            WHERE (c.id_part1 = 1 AND c.id_part2 = 1) OR (c.id_part1 = 1 AND c.id_part2 = 2) OR (c.id_part1 = 1 AND c.id_part2 = 3) OR (c.id_part1 = 1 AND c.id_part2 = 4) OR (c.id_part1 = 1 AND c.id_part2 = 5)
            */
            ParameterExpression parameter = Expression.Parameter(typeof(CompositeKeyJoin), "x");

            // Access the properties of YourEntity
            MemberExpression property1 = Expression.Property(parameter, nameof(CompositeKeyJoin.IdPart1));
            MemberExpression property2 = Expression.Property(parameter, nameof(CompositeKeyJoin.IdPart2));

            // Build the OR expression for each tuple in the collection
            Expression? combinedExpression = null;
            foreach (var (val1, val2) in tupleCollection)
            {
                // Create constant expressions for the tuple values
                ConstantExpression constant1 = Expression.Constant(val1);
                ConstantExpression constant2 = Expression.Constant(val2);

                // Create an AND expression for the tuple pair (x.Property1 == val1 && x.Property2 == val2)
                BinaryExpression condition1 = Expression.Equal(property1, constant1);
                BinaryExpression condition2 = Expression.Equal(property2, constant2);
                BinaryExpression andExpression = Expression.AndAlso(condition1, condition2);

                // Combine the AND expressions with OR
                combinedExpression = combinedExpression == null ? andExpression : Expression.OrElse(combinedExpression, andExpression);
            }

            // Return the final expression as a lambda
            return Expression.Lambda<Func<CompositeKeyJoin, bool>>(combinedExpression ?? Expression.Constant(false), parameter);
        }
    }
}

