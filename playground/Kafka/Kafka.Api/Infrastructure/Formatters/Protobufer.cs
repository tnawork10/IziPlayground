using Confluent.Kafka;
using ProtoBuf;

namespace MetricProcessing.StreamingBus.Formatters;

public sealed class Protobufer<TMessage> : IDeserializer<TMessage>, ISerializer<TMessage>
{
    public TMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, Confluent.Kafka.SerializationContext context)
    {
        return Serializer.Deserialize<TMessage>(data);
    }

    public byte[] Serialize(TMessage data, Confluent.Kafka.SerializationContext context)
    {
        using var ms = new MemoryStream();
        Serializer.Serialize(ms, data);
        return ms.ToArray();
    }
}
