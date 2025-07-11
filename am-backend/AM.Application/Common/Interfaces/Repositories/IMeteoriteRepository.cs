using AM.Application.Common.Filters;
using AM.Application.Common.Responses;
using AM.Domain.Enities;
using X.PagedList;

namespace AM.Application.Common.Interfaces.Repositories;

public interface IMeteoriteRepository : IBaseRepository<Meteorite>
{
    Task<IPagedList<MeteoritesGropedResponse>> GetGroupedByYearPagedAsync(MeteoritesSearchFilter filter, bool isAsNoTracking = false);
}
