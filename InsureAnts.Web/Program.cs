using InsureAnts.Application;
using InsureAnts.Infrastructure;
using Microsoft.AspNetCore.DataProtection;
using InsureAnts.Web;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// ====================================================================================================
// Configure Logging

builder.Logging.ClearProviders().AddConsole();

// ====================================================================================================
// Configure Services

services.ConfigureHostInfrastructure()
        .ConfigureInfrastructureServices(configuration)
        .ConfigureApplicationServices();

// ====================================================================================================
// Configure Web app

if (builder.Environment.IsDev())
{
    builder.WebHost.UseStaticWebAssets();
}

builder.Services.AddDataProtection()
    .SetApplicationName("insureAnts-app")
    .PersistKeysToFileSystem(new DirectoryInfo(@"keys"));
var app = builder.Build();
app.Logger.LogInformation("AppCreated");
app.ConfigureWebApplication();

// ====================================================================================================
// Run webapp

app.Lifetime.ApplicationStopping.Register(() => app.Logger.LogInformation("AppStopping"));
app.Logger.LogInformation("AppRunning");
try
{
    await app.RunAsync();
}
finally
{
    app.Logger.LogInformation("AppStopped");
}