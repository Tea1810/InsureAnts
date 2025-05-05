using Microsoft.Extensions.Logging;

namespace InsureAnts.Application.Logging;

public static class LoggerExtensions
{
    public static IDisposable? WithScope(this ILogger logger, string key, object? value) => logger.WithScope(new LogScopes(1) { { key, value } });

    public static IDisposable? WithScope(this ILogger logger, LogScopes scopes) => logger.BeginScope(scopes);
}