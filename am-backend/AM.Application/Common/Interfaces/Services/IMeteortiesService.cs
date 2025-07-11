using AM.Application.Common.Filters;
using AM.Application.Common.Responses;
using AM.Application.Models.Common;
using ErrorOr;

namespace AM.Application.Common.Interfaces.Services
{
    public interface IMeteortiesService
    {
        Task<ErrorOr<PagedListResponse<MeteoritesGropedResponse>>> GetMeteoritesGrouped(MeteoritesSearchFilter filter);
    }
}
