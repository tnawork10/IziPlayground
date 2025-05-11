using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using TestWebFactory.A.Services;

namespace TestWebFactory.A.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FactoryAController(IHttpClientFactory factory, HttpClient defaultHttpClient, ITypedHttpClient clientTyped) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> RequestFactoryA()
        {
            var httpClient = factory.CreateClient(Program.CLIENT_NAME);
            try
            {
                var result = await httpClient.GetAsync("FactoryB/Check");
                result.EnsureSuccessStatusCode();
                var text = await result.Content.ReadAsStringAsync();
                return Ok(value: $"BaseAddress: {httpClient.BaseAddress}. {text}");
            }
            catch (Exception ex)
            {
                return BadRequest($"BaseAddress: {httpClient.BaseAddress}{Environment.NewLine}{ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> RequestFactoryADefault()
        {
            var httpClient = defaultHttpClient;
            try
            {
                var result = await httpClient.GetAsync("FactoryB/Check");
                result.EnsureSuccessStatusCode();
                var text = await result.Content.ReadAsStringAsync();
                return Ok(value: $"BaseAddress: {httpClient.BaseAddress}. {text}");
            }
            catch (Exception ex)
            {
                return BadRequest($"BaseAddress: {httpClient.BaseAddress}{Environment.NewLine}{ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> RequestFactoryATyped()
        {
            var result = await clientTyped.SendRequestsAsync();
            return Ok(result);

         
        }
    }
}
