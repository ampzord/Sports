namespace Sports.Domain.PlayerAggregate;

using Sports.Domain.Common.Models;
using Sports.Domain.PlayerAggregate.Enums;
using Sports.Domain.PlayerAggregate.ValueObjects;
using Sports.Domain.TeamAggregate;
using Sports.Domain.TeamAggregate.ValueObjects;

public class Player : Entity<PlayerId>
{
    private Player(PlayerId id, string name, PlayerPosition position, TeamId teamId) : base(id)
    {
        Name = name;
        Position = position;
        TeamId = teamId;
    }

    public string Name { get; private set; }

    public PlayerPosition Position { get; private set; }

    public TeamId TeamId { get; private set; }

    public Team Team { get; private set; } = null!;

    public static Player Create(string name, PlayerPosition position, TeamId teamId) =>
        new(PlayerId.CreateUnique(), name, position, teamId);

    public void Update(string name, PlayerPosition position, TeamId teamId)
    {
        Name = name;
        Position = position;
        TeamId = teamId;
    }

#pragma warning disable CS8618
    private Player() { }
#pragma warning restore CS8618
}
