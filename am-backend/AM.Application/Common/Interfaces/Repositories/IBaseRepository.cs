using AM.Domain.Enities;
using EFCore.BulkExtensions;

namespace AM.Application.Common.Interfaces.Repositories;

public interface IBaseRepository<TEntity> where TEntity : Entity
{
    Task BulkInsertOrUpdateAsync(IEnumerable<TEntity> entities, Action<BulkConfig>? bulkConfig = null);
    Task<List<TEntity>> GetAllAsListAsync(bool isAsNoTracking = false, CancellationToken cancellationToken = default);
}
