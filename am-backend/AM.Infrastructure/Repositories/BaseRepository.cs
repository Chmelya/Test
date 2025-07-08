using AM.Application.Common.Interfaces.Repositories;
using AM.Domain.Enities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AM.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity>(ApplicationDbContext context) : IBaseRepository<TEntity>
    where TEntity : Entity
{
    protected DbSet<TEntity> Get(bool isAsNoTracking = false)
    {
        DbSet<TEntity> dbSet = context.Set<TEntity>();

        if (isAsNoTracking)
        {
            dbSet.AsNoTracking();
        }

        return dbSet;
    }

    protected IQueryable<TEntity> GetAsSplitable(bool isAsNoTracking = false)
    {
        return Get(isAsNoTracking).AsSplitQuery();
    }

    public async Task<List<TEntity>> GetAllAsListAsync(bool isAsNoTracking = false, CancellationToken cancellationToken = default)
    {
        return await GetAsSplitable(isAsNoTracking).ToListAsync(cancellationToken);
    }
}
