using CustomMiddlewareClass.CustomMiddlware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<MyCustomMiddlware>();
var app = builder.Build();


//MiddleWare1
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("MW 01");
    await next(context);
});

//MiddleWare2
app.UseMiddleware<MyCustomMiddlware>();

//MiddleWare3
app.Run(async(HttpContext context) =>
{
    await context.Response.WriteAsync("MW03");
});

app.Run();
