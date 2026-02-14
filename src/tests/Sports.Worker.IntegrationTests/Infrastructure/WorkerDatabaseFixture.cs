namespace Sports.Worker.IntegrationTests.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Sports.MatchSimulationWorker.Database;
using Sports.Domain.Entities;
using Sports.Tests.Shared;
using Testcontainers.MsSql;

public class WorkerDatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder(DatabaseHelper.SqlServerImage)
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
            DELETE FROM Team;
            DELETE FROM League;
            IF NOT EXISTS (SELECT 1 FROM League WHERE Id = 1)
                SET IDENTITY_INSERT League ON;
                INSERT INTO League (Id, Name) VALUES (1, 'Test League');
                SET IDENTITY_INSERT League OFF;
            IF NOT EXISTS (SELECT 1 FROM Team WHERE Id = 1)
                SET IDENTITY_INSERT Team ON;
                INSERT INTO Team (Id, Name, LeagueId) VALUES (1, 'Home Team', 1), (2, 'Away Team', 1);
                SET IDENTITY_INSERT Team OFF;
            DBCC CHECKIDENT ('Matches', RESEED, 0);
            """);

        for (var i = 0; i < count; i++)
        {
            context.Matches.Add(new Match
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                TotalPasses = null
            });
        }

        await context.SaveChangesAsync();
    }
}

[CollectionDefinition("Worker")]
public class WorkerCollection : ICollectionFixture<WorkerDatabaseFixture>;
