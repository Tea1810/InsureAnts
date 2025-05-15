using InsureAnts.Application;
using InsureAnts.Infrastructure;
using Microsoft.AspNetCore.DataProtection;
using InsureAnts.Web;
using InsureAnts.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("InsureAntsDbContextConnection") ?? throw new InvalidOperationException("Connection string 'InsureAntsDbContextConnection' not found.");;

builder.Services.AddDbContext<InsureAntsDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<InsureAntsDbContext>();
builder.Services.AddRazorPages();
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