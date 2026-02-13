namespace Sports.Api.UnitTests.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Sports.Api.Database;

public static class InMemoryDbContextFactory
{
    public static SportsDbContext Create(string? dbName = null)
    {
        var options = new DbContextOptionsBuilder<SportsDbContext>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options;

        return new SportsDbContext(options);
    }
}
