namespace Explore.Linq
{
    using System.Linq;
    using System.Linq.Expressions;

    public class ExploreLinq
    {
        public static void Run()
        {
            var a = true;
            var b = false;
            var c = false;
            ExpressionParamInfo(() => true);
            ExpressionParamInfo(() => a && b && c);
            var param0 = Expression.Parameter(typeof(int), "x");
            // Expression body: x + 1
            var body = Expression.Add(param0, Expression.Constant(1));
            var lambda = Expression.Lambda<Func<int, int>>(body, param0);
            // Compile and invoke
            var func = lambda.Compile();
            Console.WriteLine(func(5));  // Output: 6
        }

        public static void ExpressionParamInfo(Expression<Func<bool>> expression)
        {
            Console.WriteLine(expression.Name);
            Console.WriteLine(expression.ReturnType);
            Console.WriteLine(expression.Parameters);
            var body = expression.Body;
            switch (body)
            {
                case BinaryExpression bin:
                    Console.WriteLine($"BinaryExpression: {bin.NodeType}");

                    // Recursively print left and right sides
                    PrintExpression(bin.Left);
                    PrintExpression(bin.Right);
                    break;

                case MemberExpression member:
                    Console.WriteLine($"MemberExpression: {member.Member.Name}");

                    if (member.Expression is ConstantExpression closure)
                    {
                        // Captured variables are stored in a compiler-generated class
                        var container = closure.Value;
                        var field = member.Member;
                        var value = ((System.Reflection.FieldInfo)field).GetValue(container);
                        Console.WriteLine($"  → Value: {value}");
                    }
                    break;

                case ConstantExpression constant:
                    Console.WriteLine($"ConstantExpression: {constant.Value}");
                    break;

                default:
                    Console.WriteLine($"Other: {expression.NodeType}");
                    break;
            }
        }

        private static void PrintExpression(Expression left)
        {
            Console.WriteLine(left);
        }
    }
}
