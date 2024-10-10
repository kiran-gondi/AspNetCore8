var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async(HttpContext context) =>
{
    if(context.Request.Method == "GET")
    {
        if(context.Request.Query.ContainsKey("id"))
        {
            string id = context.Request.Query["id"];
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync($"Query string is: {id}");
        }
    }
    
});

app.Run();
