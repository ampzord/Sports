namespace Sports.MatchSimulationWorker.Database;

using Microsoft.EntityFrameworkCore;
using Sports.Shared.Configurations;
using Sports.Shared.Entities;

public class MatchSimulationDbContext(DbContextOptions<MatchSimulationDbContext> options)
    : DbContext(options)
{
    public DbSet<Match> Matches => Set<Match>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new MatchConfiguration());
    }
}
