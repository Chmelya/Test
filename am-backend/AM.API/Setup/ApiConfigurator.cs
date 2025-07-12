using AM.Application;
using AM.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AM.API.Setup;

internal static class ApiConfigurator
{
    private const string AllowForFrontend = "AllowForFrontend";

    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();
        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var connection = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddRepositories(connection);
        builder.Services.AddServices();

        var frontednUrl = builder.Configuration["CORS:FrontendUrl"]!;
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(AllowForFrontend, policy =>
            {
                policy.WithOrigins(frontednUrl)
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        return builder;
    }

    public static WebApplication PiplineSetup(this WebApplication app)
    {
        app.EnsureDbMigration();

        app.UseHttpsRedirection();
        app.UseCors(AllowForFrontend);

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }

    private static void EnsureDbMigration(this WebApplication app)
    {
        using IServiceScope serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
        context?.Database.Migrate();
    }
}
