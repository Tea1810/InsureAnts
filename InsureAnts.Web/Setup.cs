using System.Globalization;
using BlazorDownloadFile;
using InsureAnts.Web.Components;
using InsureAnts.Web.Infrastructure;
using InsureAnts.Web.Infrastructure.Interfaces;
using Microsoft.AspNetCore.CookiePolicy;
using MudBlazor;
using MudBlazor.Services;

namespace InsureAnts.Web;

internal static class Setup
{
    public static bool IsDev(this IHostEnvironment environment) => !string.IsNullOrEmpty(environment.EnvironmentName) && environment.EnvironmentName.StartsWith(Environments.Development, StringComparison.OrdinalIgnoreCase);

    public static IServiceCollection ConfigureHostInfrastructure(this IServiceCollection services)
    {
        var xdocAssemblyNames = new[]
        {
        typeof(Program).Assembly.GetName()
    };

        services.AddMemoryCache();

        services.AddRazorComponents().AddInteractiveServerComponents();
        services.ConfigureBlazor();

        services.AddScoped<IBlazorMediator, BlazorMediator>();

        return services;
    }

    public static IApplicationBuilder ConfigureWebApplication(this WebApplication app)
    {
        app.UseCultureInfo()
            .UseExceptionHandler("/error");

        if (!app.Environment.IsDev())
        {
            app.UseHsts();
        }

        app.UseCookiePolicy(new CookiePolicyOptions
        {
            HttpOnly = HttpOnlyPolicy.Always,
            Secure = CookieSecurePolicy.Always,
        });

        app.UseHttpsRedirection().UseStaticFiles();

        app.UseAntiforgery();

        app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

        return app;
    }

    private static IServiceCollection ConfigureBlazor(this IServiceCollection services)
    {
        services.AddMudServices(c =>
        {
            c.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomCenter;
            c.SnackbarConfiguration.NewestOnTop = true;
            c.SnackbarConfiguration.ShowCloseIcon = true;
            c.SnackbarConfiguration.VisibleStateDuration = 5500;
            c.SnackbarConfiguration.HideTransitionDuration = 250;
            c.SnackbarConfiguration.ShowTransitionDuration = 250;
            c.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        });

        services.AddBlazorDownloadFile();

        return services.AddScoped<BrowserTimezoneState>();
    }

    private static IApplicationBuilder UseCultureInfo(this IApplicationBuilder app)
    {
        var culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();

        culture.DateTimeFormat.DateSeparator = "-";
        culture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
        culture.DateTimeFormat.FullDateTimePattern = "dd-MM-yyyy HH:mm:ss";

        CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = culture;

        app.Use((_, next) =>
        {
            CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = culture;
            return next();
        });

        return app;
    }
}