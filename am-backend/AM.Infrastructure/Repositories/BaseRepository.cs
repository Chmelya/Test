using AM.Application.Common.Interfaces.Repositories;
using AM.Domain.Enities;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace AM.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : Entity
{
    private readonly ApplicationDbContext _context;

    protected BaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    protected ApplicationDbContext Context => _context;

    protected DbSet<TEntity> Get(bool isAsNoTracking = false)
    {
        DbSet<TEntity> dbSet = Context.Set<TEntity>();

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

    public async Task BulkInsertOrUpdateAsync(
        IEnumerable<TEntity> entities,
        Action<BulkConfig>? bulkConfig = null)
    {
        if (bulkConfig is null)
        {
            await Context.BulkInsertOrUpdateAsync(entities);
        }
        else
        {
            await Context.BulkInsertOrUpdateAsync(entities, bulkConfig);
        }
    }
}
