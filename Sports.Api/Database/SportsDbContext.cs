namespace Sports.Api.Database;

using Microsoft.EntityFrameworkCore;
using Sports.Api.Entities;

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

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(p => p.Position)
                .HasConversion<string>();

            entity.HasOne<Team>()
                .WithMany(t => t.Players)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne<League>()
                .WithMany(l => l.Teams)
                .HasForeignKey(t => t.LeagueId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<League>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.HasOne<Team>()
                .WithMany()
                .HasForeignKey(m => m.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Team>()
                .WithMany()
                .HasForeignKey(m => m.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}