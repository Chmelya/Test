using AM.Application.Common.Filters;
using AM.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AM.API.Controllers;

public class MeteoritesController(IMeteortiesService meteortiesService) : ApiController
{
    [HttpGet("GroupedByYear")]
    [ResponseCache(Duration = 3600)]
    public async Task<IActionResult> GetMetoritesGroupedList(
        [FromQuery] MeteoritesSearchFilter filter)
    {
        var result = await meteortiesService.GetMeteoritesGrouped(filter);

        return result.Match(Ok, Problem);
    }

    [HttpGet("Options/Recclasses")]
    [ResponseCache(Duration = 3600)]
    public async Task<IActionResult> GetMetoritesGroupedList()
    {
        var result = await meteortiesService.GetRecclasses();

        return result.Match(Ok, Problem);
    }
}
