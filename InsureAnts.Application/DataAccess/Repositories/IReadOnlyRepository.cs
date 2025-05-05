using InsureAnts.Domain.Abstractions;

namespace InsureAnts.Application.DataAccess.Repositories;

public interface IReadOnlyRepository<out TEntity, in TKey>
    where TEntity : class, IEntity<TKey>
{
    /// <summary>
    /// Gets a queryable entities. Updates on the resulted entities are not tracked.
    /// </summary>
    IQueryable<TEntity> All();
}