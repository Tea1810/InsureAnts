namespace InsureAnts.Application.Features.Abstractions;

internal interface IRequestInitializer<in TRequest>
{
    Task InitAsync(TRequest request, CancellationToken cancellationToken = default);
}