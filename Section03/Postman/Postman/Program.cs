using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (HttpContext context) =>
{
    StreamReader reader = new StreamReader(context.Request.Body);
     string body = await reader.ReadToEndAsync();

    //context.Response.ContentType = "text/html";
    //await context.Response.WriteAsync($"<p>{body}<p>");

    Dictionary<string, StringValues> queryStringDict = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);

    if (queryStringDict.ContainsKey("firstName"))
    {
        string firstName = queryStringDict["firstName"][0];
        await context.Response.WriteAsync(firstName  +"\n");
    }

    if (queryStringDict.ContainsKey("age"))
    {
        foreach (var age in queryStringDict["age"])
        {
            await context.Response.WriteAsync(age + "\n");
        }
    }
});

app.Run();
