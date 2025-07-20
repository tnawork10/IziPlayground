using Confluent.Kafka;
using System.Runtime.InteropServices;

namespace MetricProcessing.StreamingBus.Formatters;

internal class KeyByterizer<TKey> : ISerializer<TKey>
    where TKey: struct
{
    public byte[] Serialize(TKey data, SerializationContext context)
    {
        return GetBytes(data);
    }

    private static byte[] GetBytes(TKey str)
    {
        var size = Marshal.SizeOf(str);

        var arr = new byte[size];

        var ptr = IntPtr.Zero;

        try
        {
            ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
        return arr;
    }
}