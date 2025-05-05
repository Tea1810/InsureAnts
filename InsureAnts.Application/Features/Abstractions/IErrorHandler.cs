namespace InsureAnts.Application.Features.Abstractions;
internal interface IErrorHandler<TMessage, TResponse>
{
    ValueTask<TResponse?> TryHandleAsync(ErrorInfo<TMessage> error, CancellationToken cancellationToken);
}

internal class ErrorInfo<TMessage>
{
    public long Id { get; }

    public Exception Exception { get; }

    public TMessage Message { get; }

    public ErrorInfo(long id, Exception exception, TMessage message)
    {
        Id = id;
        Exception = exception;
        Message = message;
    }
}
