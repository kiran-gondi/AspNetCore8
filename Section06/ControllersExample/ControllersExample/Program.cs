var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddTransient<HomeController>();
builder.Services.AddControllers(); //Adds all the controller classes as services.

var app = builder.Build();
app.UseStaticFiles();

//app.UseRouting();
//app.UseEndpoints(endpoints => {  
//    endpoints.MapControllers();
//});
app.MapControllers();

app.Run();
