using CustomRouteConstraint.CustomConstraints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("months", typeof(MonthsCustomConstraint));
});

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    //sales-report/2030/apr
    endpoints.Map("sales-report/{year:int:min(1900)}/{month:months}", async context =>
    {
        int year = Convert.ToInt32(context.Request.RouteValues["year"]);
        string? month = Convert.ToString(context.Request.RouteValues["month"]);
        if (month == "apr" || month == "jul" || month == "aug")
            await context.Response.WriteAsync($"sales report year is {year} and month {month}");
        else
            await context.Response.WriteAsync($"sales report invalid month {month}");
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"No Route at {context.Request.Path}");
});

app.Run();
