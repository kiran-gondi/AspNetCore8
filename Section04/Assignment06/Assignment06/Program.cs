var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//Invoke custom middleware
app.UseLoginMiddleware();

app.Run(async context =>
{
    await context.Response.WriteAsync("No response");
});

app.Run();
