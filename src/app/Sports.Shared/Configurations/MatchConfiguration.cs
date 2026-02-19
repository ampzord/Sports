namespace Sports.Shared.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sports.Domain.MatchAggregate;
using Sports.Domain.MatchAggregate.ValueObjects;
using Sports.Domain.TeamAggregate.ValueObjects;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        var teamIdComparer = new ValueComparer<TeamId>(
            (a, b) => (object?)a != null && (object?)b != null && a.Value == b.Value,
            v => v.Value.GetHashCode(),
            v => TeamId.Create(v.Value));

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasConversion(
                id => id.Value,
                value => MatchId.Create(value),
                new ValueComparer<MatchId>(
                    (a, b) => (object?)a != null && (object?)b != null && a.Value == b.Value,
                    v => v.Value.GetHashCode(),
                    v => MatchId.Create(v.Value)))
            .ValueGeneratedNever();
        builder.Ignore(m => m.DomainEvents);
        builder.Property(m => m.HomeTeamId)
            .HasConversion(id => id.Value, value => TeamId.Create(value), teamIdComparer);
        builder.Property(m => m.AwayTeamId)
            .HasConversion(id => id.Value, value => TeamId.Create(value), teamIdComparer);

        builder.HasOne(m => m.HomeTeam)
            .WithMany()
            .HasForeignKey(m => m.HomeTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.AwayTeam)
            .WithMany()
            .HasForeignKey(m => m.AwayTeamId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
