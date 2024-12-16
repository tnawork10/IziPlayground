using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

namespace СommissioningService.IntegrationTests;

public class AdapterForOpenApi
{
    public static async Task<OpenApiDocument> FromFileAsync(string path, CancellationToken ct = default)
    {
        try
        {
            var fs = File.OpenRead(path);
            var reader = new OpenApiStreamReader();
            var result = await reader.ReadAsync(fs, ct).ConfigureAwait(false);
            OpenApiDocument openApiDocument = result.OpenApiDocument;
            return openApiDocument;
        }
        catch (IOException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static MethodInfo[] GetEndpointsHandlers(OpenApiDocument doc)
    {
        foreach (var path in doc.Paths)
        {
            // /api/Controllers/{id}
            var key = path.Key;
        }

        throw new NotImplementedException();
    }
}