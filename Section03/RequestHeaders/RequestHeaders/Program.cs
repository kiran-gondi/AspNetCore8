var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (HttpContext context) =>
{
    context.Response.Headers.ContentType = "text/html";

    if (context.Request.Headers.ContainsKey("User-Agent"))
    {
        string userAgent = context.Request.Headers["User-Agent"];
        string userAgent1 = context.Request.Headers.UserAgent;

        await context.Response.WriteAsync($"<h1>{userAgent}</h1>");
        await context.Response.WriteAsync($"<h1>{userAgent1}</h1>");
    }
    
});

app.Run();
