namespace СommissioningService.IntegrationTests;

[Collection(nameof(FixNumColl))]
public class FixturesChain : IClassFixture<Fix0>
{
    public FixturesChain(Fix0 fix0)
    {
    }

    [Fact]
    private void DoChainFix()
    {
    }
}

public class Fix0 : IClassFixture<Fix1>
{
    private readonly Fix1 fix1;

    public Fix0(Fix1 fix1)
    {
        this.fix1 = fix1;
    }
}

public class Fix1
{
}

[CollectionDefinition(nameof(FixNumColl))]
public class FixNumColl : ICollectionFixture<Fix1>
{
}