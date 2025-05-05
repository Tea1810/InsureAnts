using InsureAnts.Domain.Abstractions;
using System.Linq.Expressions;

namespace InsureAnts.Application.DataAccess.Repositories;

public interface IRepository<TEntity, in TKey> : IReadOnlyRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    /// <summary>
    /// Gets a queryable entities attaching the change tracker for future updates.
    /// </summary>
    IQueryable<TEntity> AllTracked();

    TEntity Add(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);

    void AttachRange(IEnumerable<TEntity> entities);

    TEntity Track(TEntity entity);

    Task<TEntity> TrackChild<TChildEntity>(TEntity entity, Expression<Func<TEntity, TChildEntity>> childSelector)
        where TChildEntity : class;

    TEntity Untrack(TEntity entity);

    IReadOnlyList<TEntity> UntrackAll();

    TEntity Delete(TEntity entity);

    Task<TEntity?> TryDeleteByIdAsync(TKey id);
}