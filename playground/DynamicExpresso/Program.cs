using System.Text.Json;

namespace DynamicExpresso
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var result = 10.34601130382;
            var json = await File.ReadAllTextAsync(@"C:\Users\adyco\Documents\[Personal] CSharp\IziPlayground\playground\DynamicExpresso\input.json");
            Dictionary<string, double?> formulaParameterNames = JsonSerializer.Deserialize<Dictionary<string, double?>>(json);
            var target = new Interpreter();
            target.SetDefaultNumberType(DefaultNumberType.Double);
            var parameters = formulaParameterNames.Select(x => new Parameter(x.Key, typeof(double))).ToArray();
            string formulaExpression = @"((((var0/1000)-((var1/1000)*(((var2/1000)+(var3/1000)-(var4/1000))/((var5/1000)+(var6/1000)+(var7/1000)-(var8/1000)))))+((var9/1000)*(((var10/1000)+(var11/1000)-(var12/1000))/((var13/1000)+(var14/1000)+(var15/1000)-(var16/1000)))))+(((var17/1000)-((var18/1000)*(((var19/1000)+(var20/1000)-(var21/1000))/((var22/1000)+(var23/1000)+(var24/1000)-(var25/1000)))))+((var26/1000)*(((var27/1000)+(var28/1000)-(var29/1000))/((var30/1000)+(var31/1000)+(var32/1000)-(var33/1000)))))+((var34/1000)-(var35/1000)-(var36/1000)-(var37/1000)-(var38/1000)+(var39/1000))+(((var40/1000)-(var41/1000)*(((var42/1000)+(var43/1000)-(var44/1000))/((var45/1000)+(var46/1000)+(var47/1000)-(var48/1000))))+((var49/1000)*(((var50/1000)+(var51/1000)-(var52/1000))/((var53/1000)+(var54/1000)+(var55/1000)-(var56/1000)))+(var57/1000))))+(((var58/1000)))+((((var59/1000)-(0))+((var60/1000)*(((var61/1000)+(var62/1000)-(var63/1000))/((var64/1000)+(var65/1000)+(var66/1000)-(var67/1000))))))";
            Lambda l1 = target.Parse("Math.Pow(2,2)");
            var val1 = l1.Invoke();

            Lambda lambdaFormulaExpression = target.Parse(formulaExpression, parameters);
            var parameterValues = formulaParameterNames.Select(x => new Parameter(x.Key, x.Value ?? 0.0)).ToArray();
            var resultFact = lambdaFormulaExpression.Invoke(parameterValues);
            //var resultFactDouble = double.Parse((string)resultFact);
            var resultFactDouble = (double)resultFact;
            Console.WriteLine(resultFact);
            Console.WriteLine(Math.Abs(resultFactDouble - result) < double.Epsilon);
            Console.WriteLine();
            Pow();
        }

        public static void Pow()
        {
            var perameters = new Parameter[] {
                new Parameter("Value", typeof(double), 2),
                new Parameter("Pow", typeof(double),3),
            };
            var v = Math.Pow(1, 2);
            var formulaExpression = $"Math.Pow(Value,Pow)";
            var target = new Interpreter();
            Lambda lambdaFormulaExpression = target.Parse(formulaExpression, perameters);

            var resultFact = lambdaFormulaExpression.Invoke(perameters);
            Console.WriteLine(resultFact);

            string s = $"<EXPORESSION_POW>СТЕПЕНЬ(<EXPORESSION_POW>СТЕПЕНЬ(2;3)</EXPORESSION_POW>;3)</EXPORESSION_POW>";

        }
    }
}
