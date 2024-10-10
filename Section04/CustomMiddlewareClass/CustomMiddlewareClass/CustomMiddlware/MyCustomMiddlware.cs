
namespace CustomMiddlewareClass.CustomMiddlware
{
    public class MyCustomMiddlware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("My custom middlware - STARTS");
            await next(context);
            await context.Response.WriteAsync("My custom middlware - ENDS");
        }
    }
}
