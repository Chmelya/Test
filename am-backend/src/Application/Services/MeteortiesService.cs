using AM.Application.Common.Filters;
using AM.Application.Common.Interfaces.Repositories;
using AM.Application.Common.Interfaces.Services;
using AM.Application.Common.Responses;
using AM.Application.Models.Common;
using ErrorOr;
using FluentValidation;

namespace AM.Application.Services
{
    public class MeteortiesService(
        IMeteoriteRepository meteoriteRepository,
        IRecclassRepository recclassRepository,
        IValidator<MeteoritesSearchFilter> meteoritesSearchFilterValidator
        ) : IMeteortiesService
    {
        public async Task<ErrorOr<List<DropdownResponse>>> GetRecclasses()
        {
            try
            {
                return (await recclassRepository
                    .GetAllAsListAsync())
                    .Select(r => new DropdownResponse { Id = r.Id, Value = r.Name})
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error during fetching recclasess", ex);
            }
        }

        public async Task<ErrorOr<PagedListResponse<MeteoritesGropedResponse>>> GetMeteoritesGrouped(MeteoritesSearchFilter filter)
        {
            var validationResult = meteoritesSearchFilterValidator.Validate(filter);

            if (!validationResult.IsValid)
            {
                //TODO: Show all together 
                return Error.Validation(description: validationResult.Errors.FirstOrDefault()!.ErrorMessage);
            }

            try
            {
                return await meteoriteRepository
                    .GetGroupedByYearPagedAsync(filter);

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error during fetching meteorites", ex);
            }
        }
    }
}
