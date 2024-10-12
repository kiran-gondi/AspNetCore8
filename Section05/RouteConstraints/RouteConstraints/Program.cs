var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    //Default Parameters
    //Ex: products/details/1
    endpoints.Map("products/details/{id=1}", async context =>
    {
        int id = Convert.ToInt16(context.Request.RouteValues["id"]);
        await context.Response.WriteAsync($"Product details: {id}");
    });

    //Optional Parameters
    //Ex: products/details/
    endpoints.Map("productsOpt/details/{id:int?}", async context =>
    {
        if (context.Request.RouteValues.ContainsKey("id"))
        {
            int id = Convert.ToInt16(context.Request.RouteValues["id"]);
            await context.Response.WriteAsync($"Product details: {id}");
        }
        else
        {
            await context.Response.WriteAsync($"Product details: id is not supplied!");
        }
    });


    //Ex: daily-digest-report/{reportdate}
    endpoints.Map("daily-digest-report/{reportdate:datetime}", async context =>
    {
        DateTime reportDate = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);
        await context.Response.WriteAsync($"In daily-digest-report {reportDate.ToShortDateString()}");
    });

});


app.Run(async context =>
{
    context.Response.WriteAsync($"Request received at {context.Request.Path}");
});

app.Run();
