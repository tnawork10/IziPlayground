using System.IO.Pipelines;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.ActionResults
{
    public class Jss : JsonConverter<object>
    {
        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
    public class CuztomActionResult : IActionResult, IDisposable
    {
        private object someDisposable;
        private readonly Utf8JsonWriter utf8JsonWriter;
        public CuztomActionResult(object someDisposable, JsonWriterOptions options = default)
        {
            this.someDisposable = someDisposable;
            //utf8JsonWriter = new Utf8JsonWriter(utf8Json: new , options);
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await Example(context);
            //await ExampleWithjsonWriter(context);
        }

        public async Task Example(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.Headers.Add("My-custom-header", "somevall of header");
            response.StatusCode = 400;
            response.ContentType = "text/plain";
            await response.Body.FlushAsync();
            //await response.WriteAsJsonAsync("Some");
            var bytes = Encoding.UTF8.GetBytes("this is some message");
            await response.BodyWriter.WriteAsync(bytes);
            //await response.BodyWriter.FlushAsync();
            //await response.BodyWriter.CompleteAsync();

        }
        public async Task ExampleWithjsonWriter(ActionContext context)
        {
            // Get the PipeWriter from the HttpResponse
            PipeWriter bodyWriter = context.HttpContext.Response.BodyWriter;
            var response = context.HttpContext.Response;

            // Set the Content-Type and Status Code
            response.ContentType = "application/json";
            response.StatusCode = 200;

            // Configure JsonWriter options (optional)
            var jsonWriterOptions = new JsonWriterOptions
            {
                Indented = true // Set to true if you want formatted (indented) JSON
            };

            // Initialize the Utf8JsonWriter with the PipeWriter's buffer
            utf8JsonWriter.Reset(response.BodyWriter);
            using (utf8JsonWriter)
            {
                // Start writing JSON
                utf8JsonWriter.WriteStartObject();
                utf8JsonWriter.WriteString("message", "Hello, this is a custom JSON response using BodyWriter and Utf8JsonWriter!");
                utf8JsonWriter.WriteNumber("status", 200);

                // Example of writing nested objects
                utf8JsonWriter.WriteStartArray("items");

                for (int i = 1; i <= 3; i++)
                {
                    utf8JsonWriter.WriteStartObject();
                    utf8JsonWriter.WriteString("item", $"Item {i}");
                    utf8JsonWriter.WriteNumber("value", i * 10);
                    utf8JsonWriter.WriteEndObject();
                }

                utf8JsonWriter.WriteEndArray();
                utf8JsonWriter.WriteEndObject();

                // Flush the writer and ensure the data is written to the response
                await utf8JsonWriter.FlushAsync();
            }

            // Complete the bodyWriter to indicate the writing process is finished
            await bodyWriter.CompleteAsync();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
