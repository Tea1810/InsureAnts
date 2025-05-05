namespace InsureAnts.Application.Features.Abstractions;

public interface ICommand<out TResponse> : Mediator.ICommand<TResponse>;

public interface ICommandHandler<in TCommand, TResponse> : Mediator.ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>;