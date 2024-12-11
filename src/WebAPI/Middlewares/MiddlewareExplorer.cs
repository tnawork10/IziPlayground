namespace WebAPI.Middlewares
{
    public class MiddlewareExplorer
    {
        private readonly RequestDelegate _next;

        public MiddlewareExplorer(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine("Middleware before invoke");
            // before request
            await _next(context);
            // after request
            Console.WriteLine("Middleware after invoke");
        }
    }
}
