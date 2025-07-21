using Confluent.Kafka;
using MetricProcessing.StreamingBus.Formatters;

namespace Kafka.Api.Infrastructure
{
    public class KafkaAdapter<TMessage>(ILogger<KafkaAdapter<TMessage>> logger, Protobufer<TMessage> protobufer)
    {
        private readonly TimeSpan _slice = TimeSpan.FromSeconds(10);

        public void BuildConsumer()
        {
            var config = new ConsumerConfig();
            var builder = new ConsumerBuilder<Ignore, TMessage>(config);

            builder.SetValueDeserializer(protobufer).SetErrorHandler((_, e) =>
            {
                logger.LogError("{reason}", e.Reason is var reason);
            });

            var consumer = builder.Build();
            while (true)
            {
                var c = consumer.Consume(_slice);
            }
        }
    }
}
