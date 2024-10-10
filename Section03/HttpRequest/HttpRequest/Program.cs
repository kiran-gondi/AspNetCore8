var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async(HttpContext context) =>
{
    string path = context.Request.Path;
    string requestMethod = context.Request.Method;
    context.Response.Headers["Content-Type"] = "text/html";
    await context.Response.WriteAsync($"<p>{path}<p>");
    await context.Response.WriteAsync($"<p>{requestMethod}<p>");
});

app.Run();
