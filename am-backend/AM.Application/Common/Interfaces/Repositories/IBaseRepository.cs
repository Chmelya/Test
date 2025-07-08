using AM.Domain.Enities;

namespace AM.Application.Common.Interfaces.Repositories;

public interface IBaseRepository<TEntity> where TEntity : Entity
{
    Task<List<TEntity>> GetAllAsListAsync(bool isAsNoTracking = false, CancellationToken cancellationToken = default);
}
