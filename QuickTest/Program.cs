using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.Extensions.Primitives;

namespace QuickTest;
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine(JsonSerializer.Serialize(TimeSpan.MaxValue));
        var reg = new RegExReplaceForExcelFormula();
        var v2 = new ReplaceForExcelFormula();

        var input = "stack �������(3,�������(5,2))asd asd +asd a=do\r\nasda �������(3,2+asda) asda\r\nasdassd �������(3,�������(5,2))aaaa ()asldklak\r\n�������(3,�������(�������((1+1),2),2)) +++ ()asd ()";
        //var input = "�������(3,2)";
        var res = reg.ReplaceMathPow(input);
        var res2 = v2.Replace(input);
        Console.WriteLine(res2);
    }
        
    public class ReplaceForExcelFormula
    {
        public const string SEQ_OPEN = "�������(";
        public const string SEQ_CLOSE = ")";
        public const char SKIP_OPEN = '(';
        public const char SKIP_CLOSE = ')';
        public static readonly Regex regex = new Regex(@"�������\(", RegexOptions.Singleline);
        public const string REPLACE_POW = "Math.Pow(";

        public string Replace(string input)
        {
            return regex.Replace(input, REPLACE_POW);
        }
    }

    public class RegExReplaceForExcelFormula
    {
        public const string FUNC_NAME_MATH_POW = "�������";
        public readonly Regex regex = new Regex(PATTERN_MATH_POW, RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
        public const string PATTERN_MATH_POW =
            @"(?<pow>
                (?<openPow>�������\(
                )+
                (?<powinside>
                    [^()]*
                    (?<nestedPara>
                        (?<paraLeft>
                            (?<Open>\()
                            [^()]*
                        )+
                        (?<paraRight>
                            (?<BetweenPara-Open>\))
                            [^()]*
                        )+
                    )*
                )
                (?<betweenPow-openPow>\))+
            )";

        public string ReplaceMathPow(string input)
        {

            var mathces = regex.Matches(input) as IEnumerable<Match>;
            // �� ��������� mathces ������������� �� ������� � ����������� ������ ������ (����� �������)
            foreach (var match in mathces)
            {
                var grps = match.Groups;
                var betweenPow = grps.Values.FirstOrDefault(x => x.Name == "betweenPow");
                ArgumentNullException.ThrowIfNull(betweenPow);
                var span = betweenPow.ValueSpan;
            }
            return "";
        }
    }
}