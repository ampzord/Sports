using Microsoft.EntityFrameworkCore;
using Sports.MatchSimulationWorker.Database;
using Sports.Shared.Entities;
using Testcontainers.MsSql;

namespace Sports.Worker.IntegrationTests.Infrastructure;

public class WorkerDatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();

    public string ConnectionString { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        ConnectionString = _container.GetConnectionString();

        await using var context = CreateContext();
        await context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    public MatchSimulationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<MatchSimulationDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;

        return new MatchSimulationDbContext(options);
    }

    public async Task SeedMatchesAsync(int count)
    {
        await using var context = CreateContext();

        await context.Database.ExecuteSqlRawAsync(
            """
            DELETE FROM Matches;
            DBCC CHECKIDENT ('Matches', RESEED, 0);
            """);

        for (var i = 0; i < count; i++)
        {
            context.Matches.Add(new Match
            {
                HomeTeamId = 0,
                AwayTeamId = 0,
                TotalPasses = null
            });
        }

        await context.SaveChangesAsync();
    }
}

[CollectionDefinition("Worker")]
public class WorkerCollection : ICollectionFixture<WorkerDatabaseFixture>;
