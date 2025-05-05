using System.Linq.Expressions;
using InsureAnts.Application.DataAccess;
using InsureAnts.Domain.Abstractions;

namespace InsureAnts.Application;

public static class EntityExtensions
{
    public static Task<TEntity?> ByIdAsync<TEntity, TKey>(this IQueryable<TEntity> query, TKey id, CancellationToken cancellationToken = default)
        where TEntity : IEntity<TKey>
    {
        return query.ById(id).FirstOrDefaultAsync(cancellationToken);
    }

    public static IQueryable<TEntity> ById<TEntity, TKey>(this IQueryable<TEntity> query, TKey id)
        where TEntity : IEntity<TKey>
    {
        return query.Where(e => id!.Equals(e.Id));
    }

    public static Task<TEntity?> FirstOrDefaultAsync<TEntity>(this IQueryable<TEntity> query, CancellationToken cancellationToken = default)
    {
        return QueryableAsyncResolver.Instance.FirstOrDefaultAsync(query, cancellationToken);
    }

    public static Task<bool> AnyAsync<TEntity>(this IQueryable<TEntity> query, CancellationToken cancellationToken = default)
    {
        return QueryableAsyncResolver.Instance.AnyAsync(query, cancellationToken);
    }

    public static Task<int> CountAsync<TEntity>(this IQueryable<TEntity> query, CancellationToken cancellationToken = default)
    {
        return QueryableAsyncResolver.Instance.CountAsync(query, cancellationToken);
    }

    public static Task<List<TEntity>> ToListAsync<TEntity>(this IQueryable<TEntity> query, CancellationToken cancellationToken = default)
    {
        return QueryableAsyncResolver.Instance.ToListAsync(query, cancellationToken);
    }

    public static Task<Dictionary<TKey, TEntity>> ToDictionaryAsync<TKey, TEntity>(this IQueryable<TEntity> query, Func<TEntity, TKey> keySelector, CancellationToken cancellationToken = default)
        where TKey : notnull
    {
        return QueryableAsyncResolver.Instance.ToDictionaryAsync(query, keySelector, cancellationToken);
    }

    public static IQueryable<TEntity> Include<TEntity, TProperty>(this IQueryable<TEntity> query, Expression<Func<TEntity, TProperty>> navigationPropertyPath, bool asSplitQuery = false)
        where TEntity : class
    {
        return QueryableAsyncResolver.Instance.Include(query, navigationPropertyPath, asSplitQuery);
    }
}