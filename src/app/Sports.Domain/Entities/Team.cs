namespace Sports.Domain.Entities;

using Sports.Domain.Common.Models;

public class Team : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;

    public Guid LeagueId { get; set; }

    public League League { get; set; } = null!;

    public ICollection<Player> Players { get; set; } = [];

    public static Team Create(string name, Guid leagueId) =>
        new() { Id = Guid.CreateVersion7(), Name = name, LeagueId = leagueId };

    public static Team Create(Guid id, string name, Guid leagueId) =>
        new() { Id = id, Name = name, LeagueId = leagueId };
}
