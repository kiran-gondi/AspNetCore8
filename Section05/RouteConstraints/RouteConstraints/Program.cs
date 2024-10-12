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
    /*endpoints.Map("productsOpt/details/{id:int?}", async context =>
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
    });*/


    //Ex: daily-digest-report/{reportdate}
    endpoints.Map("daily-digest-report/{reportdate:datetime}", async context =>
    {
        DateTime reportDate = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);
        await context.Response.WriteAsync($"In daily-digest-report {reportDate.ToShortDateString()}");
    });

    //Ex: cities/cityid
    //cities/cityid/54A48085-12D3-435E-A70E-8F7CAF212093
    //{54A48085-12D3-435E-A70E-8F7CAF212093}
    endpoints.Map("cities/{cityid:guid}", async context =>
    {
        Guid cityId = Guid.Parse(Convert.ToString(context.Request.RouteValues["cityid"])!);
        await context.Response.WriteAsync($"City information - {cityId}");
    });

    /*endpoints.Map("employee/profile/{EmployeeName:minlength(3):maxlength(6)=Bill}", async context =>
    {
        string? routeParamEmployee = Convert.ToString(context.Request.RouteValues["employeeName"]);
        await context.Response.WriteAsync($"In Employee profile of {routeParamEmployee}");
    });*/
    endpoints.Map("employee/profile/{EmployeeName:length(3, 6):alpha=Bill}", async context =>
    {
        string? routeParamEmployee = Convert.ToString(context.Request.RouteValues["employeeName"]);
        await context.Response.WriteAsync($"In Employee profile of {routeParamEmployee}");
    });

    /*endpoints.Map("productsOpt/details/{id:int:min(1):max(1000)?}", async context =>
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
    });*/

    endpoints.Map("productsOpt/details/{id:int:range(1,1000)?}", async context =>
    {
        if (context.Request.RouteValues.ContainsKey("id"))
        {
            int id = Convert.ToInt32(context.Request.RouteValues["id"]);
            await context.Response.WriteAsync($"Product details: {id}");
        }
        else
        {
            await context.Response.WriteAsync($"Product details: id is not supplied!");
        }
    });

    //sales-report/2030/apr
    endpoints.Map("sales-report/{year:int:min(1900)}/{month:regex(^(apr|jul|aug)$)}", async context =>
    {
        int year = Convert.ToInt32(context.Request.RouteValues["year"]);
        string? month = Convert.ToString(context.Request.RouteValues["month"]);
        if(month == "apr" || month == "jul" || month == "aug")
            await context.Response.WriteAsync($"sales report year is {year} and month {month}");
        else
            await context.Response.WriteAsync($"sales report invalid month {month}");
    });

});


app.Run(async context =>
{
    //await context.Response.WriteAsync($"Request received at {context.Request.Path}");
    await context.Response.WriteAsync($"No Route matched at {context.Request.Path}");
});

app.Run();
