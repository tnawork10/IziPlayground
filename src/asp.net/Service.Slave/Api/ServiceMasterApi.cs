using System.Net.Http;
using System;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace Service.Slave
{
    public class ServiceMasterApi
    {
        private readonly HttpClient client;
        public ServiceMasterApi(HttpClient client)
        {
            this.client = client;
            client.BaseAddress = new Uri("http://localhost:5014");

        }

        public async IAsyncEnumerable<T?> RequestPartialWithGet<T>()
        {
            var list = new List<long>();
            for (int i = 0; i < 100; i++)
            {
                list.Add(i);
            }
            var uri = $"/PartialFlush/GetLongs?count={100}";


            using var stream = await this.client.GetStreamAsync(uri);
            using var reader = new StreamReader(stream);
            string boundary = "--boundary123"; // Must match the server boundary

            while (!reader.EndOfStream)
            {
                string? line = await reader.ReadLineAsync();
                if (line == null) continue;

                if (line.StartsWith(boundary))
                {
                    // Read and skip headers
                    while (!string.IsNullOrEmpty(line = await reader.ReadLineAsync())) { }

                    // Read the JSON content
                    string? jsonContent = await reader.ReadLineAsync();
                    if (jsonContent != null)
                    {
                        var obj = JsonSerializer.Deserialize<T>(jsonContent);
                        yield return obj;
                    }
                }
            }
        }


        public async IAsyncEnumerable<T?> RequestPartialWithPost<T>()
        {
            var list = new List<long>();
            for (int i = 0; i < 5; i++)
            {
                list.Add(i);
            }
            var uri = $"/PartialFlush/PostLongs";
            string boundary = "--boundary123"; // Must match the server boundary

            var strContent = JsonContent.Create(list);
            using var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = strContent
            };

            //var response = this.client.Send(request);
            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            Console.WriteLine("Unblocked");
            using var reader = new StreamReader(await response.Content.ReadAsStreamAsync());
            //string boundary = "--boundary123"; // Must match the server boundary

            while (!reader.EndOfStream)
            {
                string? line = await reader.ReadLineAsync();
                if (line == null) continue;

                if (line.StartsWith(boundary))
                {
                    // Read and skip headers
                    while (!string.IsNullOrEmpty(line = await reader.ReadLineAsync())) { }

                    // Read the JSON content
                    string? jsonContent = await reader.ReadLineAsync();
                    if (jsonContent != null)
                    {
                        var obj = JsonSerializer.Deserialize<T>(jsonContent);
                        yield return obj;
                    }
                }
            }
            Console.WriteLine("Response recieved");
        }
    }
}