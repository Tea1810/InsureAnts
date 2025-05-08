namespace InsureAnts.Application.Features.Abstractions;

public interface IResponse
{
    bool IsSuccess { get; }

    bool HasWarnings { get; }

    string Message { get; }

    Exception? Exception { get; }
}

public interface IResponse<out T> : IResponse
{
    T? Data { get; }
}

internal class Response : IResponse
{
    public bool IsSuccess { get; private init; }

    public bool HasWarnings { get; private init; }

    public string Message { get; }

    public Exception? Exception { get; private init; }

    private Response(string message)
    {
        Message = message;
    }

    public static Response Failure(string message, Exception? exception = null) => new(message) { Exception = exception };

    public static Response Success(string message, bool hasWarnings = false) => new(message) { IsSuccess = true, HasWarnings = hasWarnings };

    public static implicit operator ValueTask<IResponse>(Response response) => ValueTask.FromResult<IResponse>(response);
}

internal class Response<T> : IResponse<T>
{
    private readonly IResponse _response;

    public bool IsSuccess => _response.IsSuccess;

    public bool HasWarnings => _response.HasWarnings;

    public string Message => _response.Message;

    public Exception? Exception => _response.Exception;

    public T? Data { get; }

    public Response(IResponse response, T? data)
    {
        Data = data;
        _response = response;
    }

    public static implicit operator ValueTask<IResponse<T>>(Response<T> response) => ValueTask.FromResult<IResponse<T>>(response);
}

internal static class ResponseExtensions
{
    public static Response<T> For<T>(this IResponse response, T? data = default)
    {
        return new Response<T>(response, data);
    }
}