var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Use(async (context, next) =>
{
    Endpoint? endPoint = context.GetEndpoint();
    if(endPoint != null)
        await context.Response.WriteAsync($"Endpoint: {endPoint.DisplayName}\n");
    await next(context);
});

//enable routing
app.UseRouting();

app.Use(async (context, next) =>
{
    Endpoint? endPoint = context.GetEndpoint();
    if (endPoint != null)
        await context.Response.WriteAsync($"Endpoint: {endPoint.DisplayName}\n");
    await next(context);
});

//create end points
app.UseEndpoints(endpoints =>
{
    //add your end points
    //endpoints.MapControllers();
    endpoints.Map("map1", async (context) =>
    {
        await context.Response.WriteAsync("In Map 1");
    });

    endpoints.MapGet("map2", async (context) =>
    {
        await context.Response.WriteAsync("In Map 2");
    });

    endpoints.MapPost("map3", async (context) =>
    {
        await context.Response.WriteAsync("In Map 3");
    });

});

app.Run(async context =>
{
    await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});

app.Run();
