using InsureAnts.Application.Features.Abstractions;

namespace InsureAnts.Web.Infrastructure.Interfaces;

public interface IBlazorMediator
{
    ValueTask<TResponse> Send<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default);

    ValueTask<TResponse> Send<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default);
}