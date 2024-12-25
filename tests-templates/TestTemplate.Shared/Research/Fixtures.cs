using Xunit;
using Xunit.Abstractions;

namespace СommissioningService.IntegrationTests;

[Collection(nameof(FixCollection))]
public class Fixtures : IClassFixture<FixB>
{
    private readonly FixA a;
    private readonly FixB b;
    private readonly FixC c;

    public Fixtures(FixA a, FixB b, FixC c)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);
        ArgumentNullException.ThrowIfNull(c);
        this.a = a;
        this.b = b;
        this.c = c;
    }

    [Fact]
    private void DoSomething()
    {
        Assert.Same(b.a, a);
        Assert.Same(b.c, c);
    }
}

public class FixA
{
}

public class FixC
{
}

public class FixB
{
    public readonly FixA a;
    public readonly FixC c;


    public FixB(FixA a, FixC c)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(c);
        this.a = a;
        this.c = c;
        // ArgumentNullException.ThrowIfNull(helper);
    }
}

[CollectionDefinition(nameof(FixCollection))]
public class FixCollection : ICollectionFixture<FixA>, ICollectionFixture<FixC>
{

}