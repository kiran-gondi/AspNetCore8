var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//Middle Ware 1
app.Use(async (HttpContext context, RequestDelegate next) => {
    await context.Response.WriteAsync("Middle Ware 1");
    await next(context);
});

//Middle Ware 2
app.Use(async (HttpContext context, RequestDelegate next) => {
    await context.Response.WriteAsync("Middle Ware 2");
    await next(context);
});

//Middle Ware 3
app.Run(async (HttpContext context) => {
    await context.Response.WriteAsync("Middle Ware 3");
});

app.Run();
