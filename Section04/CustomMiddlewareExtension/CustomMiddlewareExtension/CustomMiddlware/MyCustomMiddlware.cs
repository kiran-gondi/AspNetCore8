namespace CustomMiddlewareExtension.CustomMiddlware
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

    public static class CustomMiddlewareExtension
    {
        public static IApplicationBuilder UseMyCustomMiddlware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MyCustomMiddlware>();
        }
    }
}
