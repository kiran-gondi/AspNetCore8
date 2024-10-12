var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();

Dictionary<int, string> contriesDict = new Dictionary<int, string>();
contriesDict.Add(1, "United States");
contriesDict.Add(2, "Canada");
contriesDict.Add(3, "United Kingdom");
contriesDict.Add(4, "India");
contriesDict.Add(5, "Japan");

#pragma warning disable ASP0014 // Suggest using top level route registrations
app.UseEndpoints(endpoints => {

    endpoints.MapGet("/countries", async context =>
    {
        //await context.Response.WriteAsync($"Total countries {contriesDict.Count} to display: \n");
        foreach (var key in contriesDict.Keys) { 
        await context.Response.WriteAsync($"{contriesDict[key]}\n");
        }
    });

    //When request path is "countries/{countryID}"
    endpoints.MapGet("/countries/{countryID:min(101)}", async context =>
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("The CountryID should be between 1 and 100 - min");
    });

    endpoints.MapGet("/countries/{countryID:int:range(1,100)}", async context =>
    {
        if (context.Request.RouteValues.ContainsKey("countryID") == false)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("The CountryID should be between 1 and 100");
        }

        //await context.Response.WriteAsync($"Total countries {contriesDict.Count} to display: \n");
        int countryId = Convert.ToInt32(context.Request.RouteValues["countryID"]);
        contriesDict.TryGetValue(countryId, out var country);
        if (!string.IsNullOrEmpty(country))
        {
            await context.Response.WriteAsync($"Country by id is {country}");
        }
        else
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync($"[No country]");
        }
    });

});
#pragma warning restore ASP0014 // Suggest using top level route registrations


app.Run(async context =>
{
    await context.Response.WriteAsync($"No route at {context.Request.Path}");
});

app.Run();
