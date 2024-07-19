using Confluent.Kafka;
//using Confluent.Kafka.Streams;
using Streamiz.Kafka;
using Streamiz.Kafka.Net.SerDes;
using Streamiz.Kafka.Net.Stream;
using Streamiz.Kafka.Net.Table;
using Streamiz.Kafka.Net;

namespace Kafka
{
public class Class1
{
        public async Task Run()
        {
            var str = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fffffff")
            var config = new StreamConfig<StringSerDes, StringSerDes>();
            config.ApplicationId = "test-app";
            config.BootstrapServers = "localhost:9092";

            StreamBuilder builder = new StreamBuilder();

            var kstream = builder.Stream<string, string>("stream");
            var ktable = builder.Table("table", InMemory.As<string, string>("table-store"));

            kstream.Join(ktable, (v, v1) => $"{v}-{v1}")
                   .To("join-topic");

            Topology t = builder.Build();
            KafkaStream stream = new KafkaStream(t, config);

            Console.CancelKeyPress += (o, e) => {
                stream.Dispose();
            };

            await stream.StartAsync();
        }
    }
}