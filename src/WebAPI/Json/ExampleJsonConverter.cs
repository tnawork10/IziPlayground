using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebAPI.Json
{
    [JsonConverter(typeof(ExampleJsonConverter))]
    public class ValueObjectOfString
    {
        public string? Value { get; set; }
    }

    public class ExampleJsonConverter : System.Text.Json.Serialization.JsonConverter<ValueObjectOfString>
    {
        public override ValueObjectOfString? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new Exception("to detect middleware");
        }

        public override void Write(Utf8JsonWriter writer, ValueObjectOfString value, JsonSerializerOptions options)
        {
            throw new Exception("to detect middleware");
        }
    }
}
