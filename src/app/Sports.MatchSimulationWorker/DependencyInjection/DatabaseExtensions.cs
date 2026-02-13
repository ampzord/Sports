namespace Sports.MatchSimulationWorker.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using Sports.MatchSimulationWorker.Database;
using Sports.Shared.Configuration;

public static class DatabaseExtensions
{
    public static IServiceCollection RegisterDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStrings.SportsDB)
            ?? throw new InvalidOperationException(
                $"Connection string '{ConnectionStrings.SportsDB}' not found.");

        services.AddDbContext<MatchSimulationDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }
}
