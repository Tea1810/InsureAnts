using System.Linq.Expressions;

namespace InsureAnts.Application.DataAccess.Interfaces;

public interface IQueryableAsyncResolver
{
    Task<bool> AnyAsync<TEntity>(IQueryable<TEntity> query, CancellationToken cancellationToken = default);

    Task<int> CountAsync<TEntity>(IQueryable<TEntity> query, CancellationToken cancellationToken = default);

    Task<TEntity?> FirstOrDefaultAsync<TEntity>(IQueryable<TEntity> query, CancellationToken cancellationToken = default);

    Task<List<TEntity>> ToListAsync<TEntity>(IQueryable<TEntity> query, CancellationToken cancellationToken = default);

    Task<Dictionary<TKey, TEntity>> ToDictionaryAsync<TKey, TEntity>(IQueryable<TEntity> query, Func<TEntity, TKey> keySelector, CancellationToken cancellationToken = default)
        where TKey : notnull;

    IQueryable<TEntity> Include<TEntity, TProperty>(IQueryable<TEntity> query, Expression<Func<TEntity, TProperty>> navigationPropertyPath, bool asSplitQuery)
        where TEntity : class;
}