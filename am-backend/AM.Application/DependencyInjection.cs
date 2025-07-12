using AM.Application.Common.Interfaces.Services;
using AM.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AM.Application;


public static class DependencyInjection
{
    public static void AddServices(
        this IServiceCollection services)
    {
        services.AddScoped<IMeteortiesService, MeteortiesService>();
    }
}


