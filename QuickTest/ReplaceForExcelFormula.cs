using System.Text.RegularExpressions;

namespace QuickTest;


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
