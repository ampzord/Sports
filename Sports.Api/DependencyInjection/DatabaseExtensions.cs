namespace Sports.Api.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public static class DatabaseExtensions
{
    public static IServiceCollection RegisterDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<SportsDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")
            )
        );

        return services;
    }
}