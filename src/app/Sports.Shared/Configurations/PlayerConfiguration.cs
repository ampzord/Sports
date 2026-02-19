namespace Sports.Shared.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sports.Domain.PlayerAggregate;
using Sports.Domain.PlayerAggregate.ValueObjects;
using Sports.Domain.TeamAggregate.ValueObjects;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PlayerId.Create(value),
                new ValueComparer<PlayerId>(
                    (a, b) => (object?)a != null && (object?)b != null && a.Value == b.Value,
                    v => v.Value.GetHashCode(),
                    v => PlayerId.Create(v.Value)))
            .ValueGeneratedNever();
        builder.Ignore(p => p.DomainEvents);
        builder.Property(p => p.TeamId)
            .HasConversion(
                id => id.Value,
                value => TeamId.Create(value),
                new ValueComparer<TeamId>(
                    (a, b) => (object?)a != null && (object?)b != null && a.Value == b.Value,
                    v => v.Value.GetHashCode(),
                    v => TeamId.Create(v.Value)));
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(p => p.Position)
            .HasConversion<string>();
        builder.HasIndex(p => p.Name)
            .IsUnique();

        builder.HasOne(p => p.Team)
            .WithMany(t => t.Players)
            .HasForeignKey(p => p.TeamId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
