namespace ApacheCassandra
{
    using Cassandra;
    using Cassandra.DataStax.Auth;
    using OpenTelemetry;
    using OpenTelemetry.Instrumentation.Cassandra;


    public class TestWritter
    {
        ICluster cluster = Cluster.Builder()
        .AddContactPoint("127.0.0.1")
        .WithOpenTelemetryMetrics()
        .WithAuthProvider(new DseGssapiAuthProvider())
        .Build();
        //OpenTelemetry.Instrumentation.Cassandra.CassandraBuilderExtensions.
    }
}