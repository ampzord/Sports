namespace Sports.Shared.Configurations;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sports.Domain.Entities;

public class LeagueConfiguration : IEntityTypeConfiguration<League>
{
    public void Configure(EntityTypeBuilder<League> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).ValueGeneratedNever();
        builder.Ignore(l => l.DomainEvents);
        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasIndex(l => l.Name)
            .IsUnique();
    }
}
