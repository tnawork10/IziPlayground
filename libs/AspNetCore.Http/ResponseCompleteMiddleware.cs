using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Common
{
    public class ResponseCompleteMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Func<object, Task> disposingFunc;

        public ResponseCompleteMiddleware(RequestDelegate next, IDisposersRegistry disposers)
        {
            _next = next;
            this.disposingFunc = ExecuteDisposeAsync;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //context.objec
            context.Response.OnCompleted(disposingFunc, null!);
            //context.Response.RegisterForDisposeAsync();
            //context.Response.
            await _next(context);
        }

        private Task ExecuteDisposeAsync(object state)
        {
            return Task.CompletedTask;
        }
    }

    public interface IDisposersRegistry
    {

    }
    public class DisposersRegistry : IDisposersRegistry
    {

    }
}
