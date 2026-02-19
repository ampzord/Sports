namespace Sports.Api.Database;

using Microsoft.EntityFrameworkCore;
using Sports.Domain.Entities;
using Sports.Shared.Configurations;

public class SportsDbContext : DbContext
{
    public SportsDbContext(DbContextOptions<SportsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Player> Players => Set<Player>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<League> Leagues => Set<League>();
    public DbSet<Match> Matches => Set<Match>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PlayerConfiguration());
        modelBuilder.ApplyConfiguration(new TeamConfiguration());
        modelBuilder.ApplyConfiguration(new LeagueConfiguration());
        modelBuilder.ApplyConfiguration(new MatchConfiguration());
    }
}