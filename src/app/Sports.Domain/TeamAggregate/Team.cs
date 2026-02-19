namespace Sports.Domain.TeamAggregate;

using Sports.Domain.Common.Models;
using Sports.Domain.LeagueAggregate;
using Sports.Domain.LeagueAggregate.ValueObjects;
using Sports.Domain.PlayerAggregate;
using Sports.Domain.TeamAggregate.ValueObjects;

public class Team : Entity<TeamId>
{
    private readonly List<Player> _players = [];

    private Team(TeamId id, string name, LeagueId leagueId) : base(id)
    {
        Name = name;
        LeagueId = leagueId;
    }

    public string Name { get; private set; }

    public LeagueId LeagueId { get; private set; }

    public League League { get; private set; } = null!;

    public IReadOnlyCollection<Player> Players => _players.AsReadOnly();

    public static Team Create(string name, LeagueId leagueId) =>
        new(TeamId.CreateUnique(), name, leagueId);

    public void Update(string name, LeagueId leagueId)
    {
        Name = name;
        LeagueId = leagueId;
    }

#pragma warning disable CS8618
    private Team() { }
#pragma warning restore CS8618
}
