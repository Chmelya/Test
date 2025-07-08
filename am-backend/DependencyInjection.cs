public static class DependencyInjection
{
    public static void AddRepositories(
        this IServiceCollection services,
        string? connection)
    {
        services.AddDbContext<ApplicationDbContext>(options => options
                        .UseSqlServer(connection));

        services.AddScoped<IUserRepository, UserRepository>();
    }
}
