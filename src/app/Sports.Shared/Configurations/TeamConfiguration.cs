namespace Sports.Shared.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sports.Domain.LeagueAggregate.ValueObjects;
using Sports.Domain.TeamAggregate;
using Sports.Domain.TeamAggregate.ValueObjects;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .HasConversion(
                id => id.Value,
                value => TeamId.Create(value),
                new ValueComparer<TeamId>(
                    (a, b) => (object?)a != null && (object?)b != null && a.Value == b.Value,
                    v => v.Value.GetHashCode(),
                    v => TeamId.Create(v.Value)))
            .ValueGeneratedNever();
        builder.Ignore(t => t.DomainEvents);
        builder.Property(t => t.LeagueId)
            .HasConversion(
                id => id.Value,
                value => LeagueId.Create(value),
                new ValueComparer<LeagueId>(
                    (a, b) => (object?)a != null && (object?)b != null && a.Value == b.Value,
                    v => v.Value.GetHashCode(),
                    v => LeagueId.Create(v.Value)));
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasIndex(t => t.Name)
            .IsUnique();

        builder.HasOne(t => t.League)
            .WithMany(l => l.Teams)
            .HasForeignKey(t => t.LeagueId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
