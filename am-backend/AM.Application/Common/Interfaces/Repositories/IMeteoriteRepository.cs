using AM.Domain.Enities;

namespace AM.Application.Common.Interfaces.Repositories;

public interface IMeteoriteRepository : IBaseRepository<Meteorite>
{
    Task BulkInsertAsync(IEnumerable<Meteorite> incomingEntities);
}
