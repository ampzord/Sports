namespace Sports.Api.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;
using Sports.Shared.Configuration;

public static class DatabaseExtensions
{
    public static IServiceCollection RegisterDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<SportsDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString(ConnectionStrings.SportsDB)
            )
        );

        return services;
    }
}