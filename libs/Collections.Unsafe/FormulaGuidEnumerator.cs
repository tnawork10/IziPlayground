namespace Formulas;

using System.Collections;

public struct FormulaGuidEnumerator : IEnumerator<GuidStringRange>
{
    private string expression;
    public GuidStringRange Current { get; private set; }

    public int offset;
    public int length;
    public int count;
    public readonly int lengthTotal;

    public FormulaGuidEnumerator(ExpressionWithGuid expression)
    {
        this.expression = expression;
        this.length = 0;
        this.lengthTotal = this.expression.Length;
        offset = default;
        length = -1;
        count = default;
    }

    public bool MoveNext()
    {
        bool isOpened = false;
        bool isClosed = false;
        var openOffset = offset;
        length = 0;

        for (int i = openOffset; i < lengthTotal; i++)
        {
            if (expression[i] == '[')
            {
                openOffset = i;
                isOpened = true;
                continue;
            }
            if (isOpened)
            {
                if (expression[i] == ']')
                {
                    length = i - openOffset + 1;
                    isClosed = true;
                    offset = i + 1;
                    break;
                }
            }
        }
        if (isOpened && isClosed)
        {
            var slice = expression.AsSpan().Slice(openOffset + 1, length - 2);
            var sliceToParse = slice;

            if (slice.Contains('_'))
            {
                var index = slice.LastIndexOf('_');
                sliceToParse = sliceToParse.Slice(0, index);
            }

            if (Guid.TryParse(sliceToParse, out var guid))
            {
                var result = new GuidStringRange(openOffset, length, guid);
#if DEBUG
                result.SetDebugString(new string(slice), new string(expression.AsSpan().Slice(openOffset, length)));
#endif
                Current = result;
                count++;
                return true;
            }
        }
        else
        {
            offset = lengthTotal;
        }
        return false;
    }

    public void Reset()
    {
        this.length = 0;
        offset = default;
        length = -1;
        count = default;
    }

    object IEnumerator.Current { get => throw new NotSupportedException(); }

    public void Dispose()
    {
        expression = string.Empty;
    }
}
/// <summary>
/// диапазон с GUid от '[' до ']' включительно, где первый и последний символ это соответственно знаки открытия и закрытия
/// </summary>
public struct GuidStringRange
{
    /// <summary>
    /// Индекс открывающего символа ('[')
    /// </summary>
    public int offset;
    /// <summary>
    /// длина от '[' до ']'
    /// </summary>
    public int length;

    public Guid guid;
#if DEBUG
    public string DEBUG_STRING = string.Empty;
    public string DEBUG_STRING_FULLRANGE = string.Empty;

    public void SetDebugString(string s, string s2)
    {
        DEBUG_STRING = s;
        DEBUG_STRING_FULLRANGE = s2;
    }
#endif

    public GuidStringRange(int offset, int length, Guid guid)
    {
        this.offset = offset;
        this.length = length;
        this.guid = guid;
    }
}