using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.DataAccess.Repositories;
using InsureAnts.Domain.Abstractions;

namespace InsureAnts.Application.Features.Abstractions;

public abstract class DeleteCommand<TKey> : ICommand<IResponse<TKey>>
{
    public required TKey Id { get; set; }
}

internal abstract class DeleteCommandHandler<TCommand, TEntity, TKey> : ICommandHandler<TCommand, IResponse<TKey>>
    where TCommand : DeleteCommand<TKey>
    where TEntity : class, IEntity<TKey>
{
    protected IUnitOfWork UnitOfWork { get; }

    protected DeleteCommandHandler(IUnitOfWork unitOfWork) => UnitOfWork = unitOfWork;

    public virtual async ValueTask<IResponse<TKey>> Handle(TCommand command, CancellationToken cancellationToken)
    {
        var repository = GetRepository();

        var entity = await repository.TryDeleteByIdAsync(command.Id!);
        if (entity == null)
        {
            return Response.Failure(Texts.NotFound<TEntity, TKey>(command.Id)).For(command.Id);
        }

        await OnBeforeDelete(entity);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Response.Success(GetSuccessMessage(command)).For(command.Id);
    }

    protected virtual string GetSuccessMessage(TCommand command) => Texts.Deleted<TEntity, TKey>(command.Id);

    protected virtual Task OnBeforeDelete(TEntity entity) => Task.CompletedTask;

    protected abstract IRepository<TEntity, TKey> GetRepository();
}