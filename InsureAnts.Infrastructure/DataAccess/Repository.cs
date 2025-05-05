using InsureAnts.Application;
using InsureAnts.Application.DataAccess.Repositories;
using InsureAnts.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InsureAnts.Infrastructure.DataAccess;

internal class Repository<TEntity, TKey> : ReadOnlyRepository<TEntity, TKey>, IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    #region .ctor
    public Repository(DbContext dbContext)
        : base(dbContext)
    { }
    #endregion


    #region Public methods
    public virtual IQueryable<TEntity> AllTracked()
    {
        return DbSet.AsTracking();
    }

    public TEntity Add(TEntity entity)
    {
        var dbEntityEntry = DbContext.Entry(entity);
        if (dbEntityEntry.State != EntityState.Detached)
        {
            dbEntityEntry.State = EntityState.Added;
        }
        else
        {
            entity = DbSet.Add(entity).Entity;
        }

        return entity;
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        DbContext.AddRange(entities);
    }

    public void AttachRange(IEnumerable<TEntity> entities)
    {
        DbContext.AttachRange(entities);
    }

    public TEntity Track(TEntity entity)
    {
        var dbEntityEntry = DbContext.Entry(entity);

        if (dbEntityEntry.State == EntityState.Detached)
        {
            entity = DbSet.Attach(entity).Entity;
        }

        return entity;
    }

    public async Task<TEntity> TrackChild<TChildEntity>(TEntity entity, Expression<Func<TEntity, TChildEntity>> childSelector)
        where TChildEntity : class
    {
        var dbEntityEntry = DbContext.Entry(entity);

        var childReference = dbEntityEntry.Reference(childSelector!);

        if (!childReference.IsLoaded)
        {
            await childReference.LoadAsync();

            entity = dbEntityEntry.Entity;
        }

        return entity;
    }

    public TEntity Untrack(TEntity entity)
    {
        DbSet.Untrack(entity);

        return entity;
    }

    public IReadOnlyList<TEntity> UntrackAll()
    {
        return DbSet.Local.Select(Untrack).ToList();
    }

    public TEntity Delete(TEntity entity)
    {
        var dbEntityEntry = DbContext.Entry(entity);
        if (dbEntityEntry.State != EntityState.Deleted)
        {
            dbEntityEntry.State = EntityState.Deleted;
        }

        return dbEntityEntry.Entity;
    }

    public async Task<TEntity?> TryDeleteByIdAsync(TKey id)
    {
        var entity = await AllTracked().ByIdAsync(id);
        return entity == null ? null : Delete(entity);
    }
    #endregion
}