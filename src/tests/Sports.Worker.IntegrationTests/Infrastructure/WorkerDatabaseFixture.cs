namespace Sports.Worker.IntegrationTests.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Sports.MatchSimulationWorker.Database;
using Sports.Domain.MatchAggregate;
using Sports.Domain.TeamAggregate.ValueObjects;
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

        var leagueId = Guid.CreateVersion7();
        var homeTeamId = Guid.CreateVersion7();
        var awayTeamId = Guid.CreateVersion7();

        await context.Database.ExecuteSqlRawAsync(
            """
            DELETE FROM Matches;
            DELETE FROM Team;
            DELETE FROM League;
            """);

        await context.Database.ExecuteSqlRawAsync(
            "INSERT INTO League (Id, Name) VALUES ({0}, 'Test League')", leagueId);
        await context.Database.ExecuteSqlRawAsync(
            "INSERT INTO Team (Id, Name, LeagueId) VALUES ({0}, 'Home Team', {1}), ({2}, 'Away Team', {1})",
            homeTeamId, leagueId, awayTeamId);

        for (var i = 0; i < count; i++)
        {
            context.Matches.Add(Match.Create(TeamId.Create(homeTeamId), TeamId.Create(awayTeamId)));
        }

        await context.SaveChangesAsync();
    }
}

[CollectionDefinition("Worker")]
public class WorkerCollection : ICollectionFixture<WorkerDatabaseFixture>;
