using AM.Application.Common.Filters;
using AM.Application.Common.Responses;
using AM.Application.Models.Common;
using AM.Domain.Enities;

namespace AM.Application.Common.Interfaces.Repositories;

public interface IMeteoriteRepository : IBaseRepository<Meteorite>
{
    Task<PagedListResponse<MeteoritesGropedResponse>> GetGroupedByYearPagedAsync(MeteoritesSearchFilter filter, bool isAsNoTracking = false);
}
