using InsureAnts.Application.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InsureAnts.Infrastructure.DataAccess;

internal class EFQueryableAsyncResolver : IQueryableAsyncResolver
{
    public static readonly IQueryableAsyncResolver Instance = new EFQueryableAsyncResolver();

    public Task<bool> AnyAsync<TEntity>(IQueryable<TEntity> query, CancellationToken cancellationToken = default) => EntityFrameworkQueryableExtensions.AnyAsync(query, cancellationToken);

    public Task<int> CountAsync<TEntity>(IQueryable<TEntity> query, CancellationToken cancellationToken = default) => EntityFrameworkQueryableExtensions.CountAsync(query, cancellationToken);

    public Task<TEntity?> FirstOrDefaultAsync<TEntity>(IQueryable<TEntity> query, CancellationToken cancellationToken = default) => EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(query, cancellationToken);

    public Task<List<TEntity>> ToListAsync<TEntity>(IQueryable<TEntity> query, CancellationToken cancellationToken = default) => EntityFrameworkQueryableExtensions.ToListAsync(query, cancellationToken);

    public Task<Dictionary<TKey, TEntity>> ToDictionaryAsync<TKey, TEntity>(IQueryable<TEntity> query, Func<TEntity, TKey> keySelector, CancellationToken cancellationToken = default)
        where TKey : notnull
    {
        return EntityFrameworkQueryableExtensions.ToDictionaryAsync(query, keySelector, cancellationToken);
    }

    public IQueryable<TEntity> Include<TEntity, TProperty>(IQueryable<TEntity> query, Expression<Func<TEntity, TProperty>> navigationPropertyPath, bool asSplitQuery) where TEntity : class
    {
        query = EntityFrameworkQueryableExtensions.Include(query, navigationPropertyPath);
        if (asSplitQuery)
        {
            query = query.AsSplitQuery();
        }

        return query;
    }
}