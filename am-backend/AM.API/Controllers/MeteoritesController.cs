using AM.Application.Common.Filters;
using AM.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AM.API.Controllers;

public class MeteoritesController(IMeteortiesService meteortiesService) : ApiController
{
    public async Task<IActionResult> GetMetoritesGroupedList(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortColumn = null,
        [FromQuery] string? sortOrder = null,
        [FromQuery] int? startYear = null,
        [FromQuery] int? endYear = null,
        [FromQuery] string? namePart = null,
        [FromQuery] string? recclass = null)
    {
        var filter = new MeteoritesSearchFilter
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SortColumn = sortColumn,
            SortOrder = sortOrder,
            Recclass = recclass,
            NamePart = namePart
        };

        var result = await meteortiesService.GetMeteoritesGrouped(filter);

        return result.Match(Ok, Problem);
    }
}
