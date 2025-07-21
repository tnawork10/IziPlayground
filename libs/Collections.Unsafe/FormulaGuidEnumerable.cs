namespace Formulas;

using System.Collections;

public struct FormulaGuidEnumerable : IEnumerable<GuidStringRange>
{
    private string expression;

    public FormulaGuidEnumerable(string expression)
    {
        this.expression = expression;
    }

    public IEnumerator<GuidStringRange> GetEnumerator()
    {
        return new FormulaGuidEnumerator(expression);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
