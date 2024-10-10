var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (HttpContext context) =>
{
    context.Response.Headers["MyKey1"] = "TestKey1";
    context.Response.Headers["Server"] = "Dummy Server";
    context.Response.Headers["Content-Type"] = "text/html";

    await context.Response.WriteAsync("<h2>Hello Bob2!<h2>");
    await context.Response.WriteAsync("<h1>Hello Bob1!<h1>");
});

app.Run();
