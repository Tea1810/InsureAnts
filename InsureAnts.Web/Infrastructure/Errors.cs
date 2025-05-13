using InsureAnts.Application.Features.Abstractions;

namespace InsureAnts.Web.Infrastructure;

internal static class Errors
{
    public const string AccessDenied = "Access denied.";
    public const string UnexpectedError = "Unexpected error.";

    public static IEnumerable<string> FormatMessages(this Exception exception)
    {
        yield return exception.Message;

        var innerException = exception.InnerException;
        while (innerException != null)
        {
            yield return innerException.Message;

            innerException = innerException.InnerException;
        }
    }

    public static IEnumerable<string> FormatMessages(this IResponse response)
    {
        return response.Exception != null
            ? FormatMessages(response.Exception).Prepend(response.Message)
            : new[] { response.Message };
    }
}