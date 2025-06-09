namespace Example.UnitTest.FixturesShared
{
    [CollectionDefinition(nameof(CollectionDef))]
    public class CollectionDef : ICollectionFixture<AsyncLifetimeShared>
    {

    }
}