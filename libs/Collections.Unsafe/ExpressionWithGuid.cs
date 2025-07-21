namespace Formulas;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Вместо шифров Guid.
/// </summary>
/// <example>
/// не [Расход_Азот_НЛМК_], а
/// [160a6399-ca30-4327-8bad-96e2e8c8b169]
/// </example>
public struct ExpressionWithGuid : IEquatable<string>
{
    public string Value { get; set; }

    public ExpressionWithGuid(string value)
    {
        Value = value;
    }
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is ExpressionWithGuid exp)
        {
            return exp.Value == Value;
        }
        return false;
    }
    public bool Equals(string? other)
    {
        return Value == other;
    }
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value;
    }
    public static implicit operator string(ExpressionWithGuid value) => value.Value;

    public static implicit operator ExpressionWithGuid(string value) => new ExpressionWithGuid(value);
    public static bool operator ==(ExpressionWithGuid ex, string value) => ex.Value == value;
    public static bool operator !=(ExpressionWithGuid ex, string value) => ex.Value != value;
    public static bool operator ==(string value, ExpressionWithGuid ex) => ex.Value == value;
    public static bool operator !=(string value, ExpressionWithGuid ex) => ex.Value != value;
    public FormulaGuidEnumerable ToEnumerable() => new FormulaGuidEnumerable(Value);
    public FormulaGuidEnumerator ToEnumerator() => new FormulaGuidEnumerator(Value);

    public bool IsConstantFormula()
    {
        return IsConstantFormula(this);
    }
    public static bool IsConstantFormula(ExpressionWithGuid expression)
    {
        var eble = expression.ToEnumerable();
        var result = !eble.Any();
        return result;
    }

    public ExpressionWithGuidAnalyzed Analyze()
    {
        var eble = ToEnumerable();
        var result = RecursiveAllocation();
        return result;
    }

    private ExpressionWithGuidAnalyzed RecursiveAllocation()
    {
        throw new NotImplementedException();
    }
}


public readonly struct ExpressionWithGuidAnalyzed : IDisposable
{
    public readonly ReadOnlyMemory<GuidStringRange> Value;
    private readonly GuidStringRange[] array;
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}