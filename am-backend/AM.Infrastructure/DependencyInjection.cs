using AM.Application.Common.Interfaces.Repositories;
using AM.Infrastructure.Repositories;
using AM.Infrastructure.UnitOfWork;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AM.Infrastructure;

public static class DependencyInjection
{
    public static void AddRepositories(
        this IServiceCollection services,
        string? connection)
    {
        services.AddDbContext<ApplicationDbContext>(options => options
                        .UseSqlServer(connection));

        services.AddScoped<IMeteoriteRepository, MeteoriteRepository>();
        services.AddScoped<IGeolocationRepository, GeolocationRepository>();
        services.AddScoped<IRecclassRepository, RecclassRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        services.AddEFSecondLevelCache(options =>
        {
            options.UseMemoryCacheProvider()
                   .UseCacheKeyPrefix("EF_");

            options.CacheAllQueries(CacheExpirationMode.Absolute, TimeSpan.FromMinutes(30));
        });
    }
}
