using Microsoft.VisualStudio.Web.CodeGeneration.Utils;
using Serilog.Context;

namespace WebAPI.Middlewares
{
    public class MiddlewareExplorer
    {
        private readonly RequestDelegate _next;

        public MiddlewareExplorer(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            return _next(context);
        }
    }
}
