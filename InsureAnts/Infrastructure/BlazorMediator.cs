using InsureAnts.Web.Infrastructure.Interfaces;
using Mediator;

namespace InsureAnts.Web.Infrastructure;

internal class BlazorMediator : IBlazorMediator
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public BlazorMediator(IServiceScopeFactory serviceScopeFactory) => _serviceScopeFactory = serviceScopeFactory;

    public async ValueTask<TResponse> Send<TResponse>(Application.Features.Abstractions.ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        return await mediator.Send(command, cancellationToken);
    }

    public async ValueTask<TResponse> Send<TResponse>(Application.Features.Abstractions.IQuery<TResponse> query, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        return await mediator.Send(query, cancellationToken);
    }
}