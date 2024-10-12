var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//enable routing
app.UseRouting();

//creating endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.Map("files/{filename}.{extension}", async context =>
    {
         string? routeParamfileName = Convert.ToString(context.Request.RouteValues["filename"]);
         string? routeParamFileExtension = Convert.ToString(context.Request.RouteValues["extension"]);

        await context.Response.WriteAsync($"In files - {routeParamfileName}-{routeParamFileExtension}");
    });

    endpoints.Map("employee/profile/{EmployeeName=Bill}", async context =>
    {
        string? routeParamEmployee = Convert.ToString(context.Request.RouteValues["employeeName"]);
        await context.Response.WriteAsync($"In Employee profile of {routeParamEmployee}");
    });

    //Default Parameters
    //Ex: products/details/1
    endpoints.Map("products/details/{id=1}", async context =>
    {
        int id = Convert.ToInt16(context.Request.RouteValues["id"]);
        await context.Response.WriteAsync($"Product details: {id}");
    });

    //Optional Parameters
    //Ex: products/details/
    endpoints.Map("productsOpt/details/{id?}", async context =>
    {
        if(context.Request.RouteValues.ContainsKey("id"))
        {
            int id = Convert.ToInt16(context.Request.RouteValues["id"]);
            await context.Response.WriteAsync($"Product details: {id}");
        }
        else
        {
            await context.Response.WriteAsync($"Product details: id is not supplied!");
        }
        
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});

app.Run();
