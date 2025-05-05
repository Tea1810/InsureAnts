using InsureAnts.Application.DataAccess.Repositories;
using InsureAnts.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace InsureAnts.Infrastructure.DataAccess;

internal class ReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    #region Properties
    protected readonly DbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;
    #endregion


    #region .ctor
    public ReadOnlyRepository(DbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = DbContext.Set<TEntity>();
    }
    #endregion


    #region Protected methods
    protected IReadOnlyRepository<T, TPk> GetReadOnly<T, TPk>()
        where T : class, IEntity<TPk> =>
        new ReadOnlyRepository<T, TPk>(DbContext);

    protected IRepository<T, TPk> Get<T, TPk>()
        where T : class, IEntity<TPk> =>
        new Repository<T, TPk>(DbContext);
    #endregion


    #region Public methods
    public virtual IQueryable<TEntity> All() => DbSet.AsNoTracking();
    #endregion
}