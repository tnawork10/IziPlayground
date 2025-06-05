using LazyCache;
using Microsoft.AspNetCore.Mvc;

namespace IziPlayGround.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LazyCacheController(IAppCache cache) : ControllerBase
    {


        [HttpGet]
        public async Task<IActionResult> NotCopy()
        {
            var key = "this is key 1";
            var o = new object();
            var o2 = await cache.GetOrAddAsync(key, async () =>
            {
                await Task.CompletedTask;
                return o;
            });
            var same = o == o2;
            cache.Remove(key);
            cache.Add(key, new object());
            var o3 = cache.Get<object>(key);
            var same2 = o == o3;
            return Ok(new
            {
                Same1 = same,
                Same2 = same2,
            });
        }
    }
}
