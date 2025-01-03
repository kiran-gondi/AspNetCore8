using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(
    new WebApplicationOptions()
{
    WebRootPath = "myroot"
});
var app = builder.Build();

app.UseStaticFiles(); //works with web root path (myroot)
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "mywebroot"))
});//works with "mywebroot" path


app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.Map("/", async context =>
    {
        await context.Response.WriteAsync("Hello");
    });
});

app.Run();
