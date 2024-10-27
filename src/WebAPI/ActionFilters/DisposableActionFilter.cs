using IziPlayGround.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.ActionFilters
{
    public class DisposableActionFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            IActionResult ar = context.Result;
            if (ar is OkObjectResult okObjectResult)
            {
                var v = okObjectResult.Value;
                if (v is IDisposable disposable)
                {

                }
                if (v is SomeDisposable sd)
                {
                    sd.SomeValue = 500;
                }
            }
            Console.WriteLine();
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            IActionResult ar = context.Result;
            if (ar is OkObjectResult okObjectResult)
            {
                var v = okObjectResult.Value;
                if (v is IDisposable disposable)
                {

                }
                if (v is SomeDisposable sd)
                {
                    sd.SomeValue = 1000;
                }
            }
        }
    }
}
