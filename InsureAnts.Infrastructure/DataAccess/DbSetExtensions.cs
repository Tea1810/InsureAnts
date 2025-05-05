using Microsoft.EntityFrameworkCore;

namespace InsureAnts.Infrastructure.DataAccess;

public static class DbSetExtensions
{
    public static void Untrack<T>(this DbSet<T> dbSet, T entity)
        where T : class
    {
        var dbEntityEntry = dbSet.Entry(entity);

        if (dbEntityEntry.State != EntityState.Detached)
        {
            dbEntityEntry.State = EntityState.Detached;
        }
    }
}