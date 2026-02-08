using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;
using Testcontainers.MsSql;

namespace Sports.Api.UnitTests.Infrastructure;

public class DatabaseFixture : IAsyncLifetime
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

    public SportsDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<SportsDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;

        return new SportsDbContext(options);
    }

    public async Task ResetAsync()
    {
        await using var context = CreateContext();
        await context.Database.ExecuteSqlRawAsync(
            """
            DELETE FROM Matches;
            DELETE FROM Players;
            DELETE FROM Teams;
            DELETE FROM Leagues;
            DBCC CHECKIDENT ('Matches', RESEED, 0);
            DBCC CHECKIDENT ('Players', RESEED, 0);
            DBCC CHECKIDENT ('Teams', RESEED, 0);
            DBCC CHECKIDENT ('Leagues', RESEED, 0);
            """);
    }
}

[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>;
