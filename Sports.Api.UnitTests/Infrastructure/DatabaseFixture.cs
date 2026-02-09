using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;
using Sports.Tests.Shared;
using Testcontainers.MsSql;

namespace Sports.Api.UnitTests.Infrastructure;

public class DatabaseFixture : IAsyncLifetime
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
        await context.Database.ResetAsync();
    }
}

[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>;
