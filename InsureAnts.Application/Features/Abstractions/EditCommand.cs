using Azure;
using FluentValidation;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Domain.Abstractions;

namespace InsureAnts.Application.Features.Abstractions;

public abstract class EditCommand<TModel, TEntity, TKey> : ICommand<IResponse<TEntity>>
{
    public TKey Id { get; set; } = default!;

    internal TEntity? Entity { get; set; }
}

internal abstract class EditCommandInitializer<TCommand, TModel, TEntity, TKey> : IRequestInitializer<TCommand>
    where TCommand : EditCommand<TModel, TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    protected IUnitOfWork UnitOfWork { get; }

    protected EditCommandInitializer(IUnitOfWork unitOfWork) => UnitOfWork = unitOfWork;

    protected abstract IQueryable<TEntity> GetTrackedQuery();

    public async Task InitAsync(TCommand request, CancellationToken cancellationToken = default)
    {
        var entity = await GetTrackedQuery().ByIdAsync(request.Id, cancellationToken);

        request.Entity = entity ?? throw new ValidationException(Texts.NotFound<TEntity, TKey>(request.Id));
    }
}