using Microsoft.AspNetCore.Mvc;
using WebAPI.ActionFilters;
using WebAPI.ActionResults;

namespace IziPlayGround.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [DisposableActionFilter]
    public class DisposableController : ControllerBase
    {
        [HttpGet(nameof(GetSomeValue))]
        public async Task<IActionResult> GetSomeValue()
        {
            return Ok(new SomeDisposable() { SomeValue = 300 });
        }

        [HttpGet(nameof(GetCustomIActionResult))]
        public async Task<IActionResult> GetCustomIActionResult()
        {
            return new CuztomActionResult(new SomeDisposable() { SomeValue = 55 });
        }
    }


    public class SomeDisposable : IDisposable
    {
        public int SomeValue { get; set; }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
