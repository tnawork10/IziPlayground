
namespace TestWebFactory.A.Services
{
    public interface ITypedHttpClient
    {
        Task<string> SendRequestsAsync();
    }

    public class TypedHttpClient : ITypedHttpClient
    {
        private HttpClient httpClient;

        public TypedHttpClient(HttpClient client)
        {
            this.httpClient = client;
        }

        public async Task<string> SendRequestsAsync()
        {
            try
            {
                var result = await httpClient.GetAsync("FactoryB/Check");
                result.EnsureSuccessStatusCode();
                var text = await result.Content.ReadAsStringAsync();
                return $"BaseAddress: {httpClient.BaseAddress}. {text}";
            }
            catch (Exception ex)
            {
                return $"BaseAddress: {httpClient.BaseAddress}{Environment.NewLine}{ex.Message}";
            }
        }
    }
}
