using AM.Application.Common.Interfaces.Repositories;
using AM.Domain.Enities;

namespace AM.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity>(ApplicationDbContext context) : IBaseRepository<TEntity>
    where TEntity : Entity
{
}
