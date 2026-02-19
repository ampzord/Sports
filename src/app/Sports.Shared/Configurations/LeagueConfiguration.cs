namespace Sports.Shared.Configurations;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sports.Domain.LeagueAggregate;
using Sports.Domain.LeagueAggregate.ValueObjects;

public class LeagueConfiguration : IEntityTypeConfiguration<League>
{
    public void Configure(EntityTypeBuilder<League> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id)
            .HasConversion(
                id => id.Value,
                value => LeagueId.Create(value),
                new ValueComparer<LeagueId>(
                    (a, b) => (object?)a != null && (object?)b != null && a.Value == b.Value,
                    v => v.Value.GetHashCode(),
                    v => LeagueId.Create(v.Value)))
            .ValueGeneratedNever();
        builder.Ignore(l => l.DomainEvents);
        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasIndex(l => l.Name)
            .IsUnique();
    }
}
