namespace Sports.Domain.Entities;

using Sports.Domain.Common.Models;

public class Player : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;

    public PlayerPosition Position { get; set; }

    public Guid TeamId { get; set; }

    public Team Team { get; set; } = null!;

    public static Player Create(string name, PlayerPosition position, Guid teamId) =>
        new() { Id = Guid.CreateVersion7(), Name = name, Position = position, TeamId = teamId };

    public static Player Create(Guid id, string name, PlayerPosition position, Guid teamId) =>
        new() { Id = id, Name = name, Position = position, TeamId = teamId };
}
