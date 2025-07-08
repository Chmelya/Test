using AM.Application.Common.Interfaces.Repositories;
using AM.Infrastructure.Repositories;
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
    }
}
