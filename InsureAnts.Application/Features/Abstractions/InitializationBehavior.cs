using Mediator;

namespace InsureAnts.Application.Features.Abstractions;

internal class InitializationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
{
    private readonly ICollection<IRequestInitializer<TRequest>> _initializers;

    public InitializationBehavior(IEnumerable<IRequestInitializer<TRequest>> initializers) => _initializers = (ICollection<IRequestInitializer<TRequest>>)initializers;

    public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        if (_initializers.Count > 0)
        {
            foreach (var initializer in _initializers)
            {
                await initializer.InitAsync(message, cancellationToken);
            }
        }

        return await next(message, cancellationToken);
    }
}