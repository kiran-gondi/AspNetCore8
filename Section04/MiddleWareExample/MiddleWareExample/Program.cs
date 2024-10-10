var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (HttpContext context)=>{
    await context.Response.WriteAsync("Hello1");
});

app.Run(async (HttpContext context) => {
    await context.Response.WriteAsync("Hello2");
});


app.Run();
