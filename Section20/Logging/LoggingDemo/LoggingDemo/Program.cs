var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Logger.LogDebug("debug-message");
app.Logger.LogInformation("information-message");
app.Logger.LogWarning("warning-message");
app.Logger.LogError("error-message");
app.Logger.LogCritical("critical-message");

app.MapGet("/", () => "Hello World!");

app.Run();
