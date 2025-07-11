using AM.Application.Common.Filters;
using AM.Application.Common.Interfaces.Repositories;
using AM.Application.Common.Interfaces.Services;
using AM.Application.Common.Responses;
using AM.Application.Extensions.PagedList;
using AM.Application.Models.Common;
using ErrorOr;

namespace AM.Application.Services
{
    public class MeteortiesService(IMeteoriteRepository meteoriteRepository) : IMeteortiesService
    {
        public async Task<ErrorOr<PagedListResponse<MeteoritesGropedResponse>>>  GetMeteoritesGrouped(MeteoritesSearchFilter filter)
        {
            try
            {

                var list = (await meteoriteRepository
                    .GetGroupedByYearPagedAsync(filter))
                    .ToPagedResponse();
                
                return list;

            }
            catch (Exception)
            {
                throw new InvalidOperationException("Error during fetching service requests");
            }
        }
    } 
}
