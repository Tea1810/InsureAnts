using FluentValidation;
using InsureAnts.Application.Logging;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InsureAnts.Application.Features.Abstractions;

internal sealed class ErrorHandlingBehaviour<TMessage, TResponse> : MessageExceptionHandler<TMessage, TResponse>
    where TMessage : IMessage
{
    private readonly IServiceProvider _serviceProvider;

    public ErrorHandlingBehaviour(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    protected override ValueTask<ExceptionHandlingResult<TResponse>> Handle(TMessage message, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is ValidationException)
        {
            return NotHandled;
        }

        var logger = _serviceProvider.GetService<ILoggerFactory>()!.CreateLogger("FeatureError");

        LogError(logger, message, exception, out var logId);

        var handler = _serviceProvider.GetService<IErrorHandler<TMessage, TResponse>>();
        if (handler == null)
        {
            return NotHandled;
        }

        var info = new ErrorInfo<TMessage>(logId, exception, message);
        return HandleInternal(handler, info, cancellationToken);
    }

    private static void LogError(ILogger logger, TMessage message, Exception exception, out long logId)
    {
        logId = DateTime.UtcNow.Ticks;

        using (logger.WithScope("LogId", logId))
        {
            logger.LogError(exception, "[{logId}] Error processing {messageType}", logId, message.GetType().Name);
        }
    }

    private static async ValueTask<ExceptionHandlingResult<TResponse>> HandleInternal(IErrorHandler<TMessage, TResponse> handler, ErrorInfo<TMessage> info, CancellationToken cancellationToken)
    {
        var response = await handler.TryHandleAsync(info, cancellationToken);
        if (response != null)
        {
            return ExceptionHandlingResult<TResponse>.Handled(response);
        }

        return NotHandled;
    }
}