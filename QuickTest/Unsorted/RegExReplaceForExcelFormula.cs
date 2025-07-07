using System.Text.RegularExpressions;

namespace QuickTest;


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
