namespace InsureAnts.Application.Features.Abstractions;

public interface IQuery<out TResponse> : Mediator.IQuery<TResponse>;

public interface IQueryHandler<in TQuery, TResponse> : Mediator.IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>;