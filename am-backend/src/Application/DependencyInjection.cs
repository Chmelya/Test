using AM.Application.Common.Filters;
using AM.Application.Common.Interfaces.Services;
using AM.Application.Services;
using AM.Application.Validors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AM.Application;


public static class DependencyInjection
{
    public static void AddServices(
        this IServiceCollection services)
    {
        services.AddScoped<IMeteortiesService, MeteortiesService>();

        services.AddScoped<IValidator<MeteoritesSearchFilter>, MeteoriteFilterValidator>();
    }
}


