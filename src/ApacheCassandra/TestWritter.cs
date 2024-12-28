namespace ApacheCassandra
{
    using Cassandra;
    using Cassandra.DataStax.Auth;
    using OpenTelemetry;
    //using Cassandra.OpenTelemetry;

    public class TestWritter
    {
        ICluster cluster = Cluster.Builder()
        .AddContactPoint("127.0.0.1")
        //.WithOpenTelemetryInstrumentation()
        .WithAuthProvider(new DseGssapiAuthProvider())
        .Build();
    }
}