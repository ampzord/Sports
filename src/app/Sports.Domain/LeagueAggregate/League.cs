namespace Sports.Domain.LeagueAggregate;

using Sports.Domain.Common.Models;
using Sports.Domain.LeagueAggregate.ValueObjects;
using Sports.Domain.TeamAggregate;

public class League : Entity<LeagueId>
{
    private readonly List<Team> _teams = [];

    private League(LeagueId id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public IReadOnlyCollection<Team> Teams => _teams.AsReadOnly();

    public static League Create(string name) =>
        new(LeagueId.CreateUnique(), name);

    public void UpdateName(string name) => Name = name;

#pragma warning disable CS8618
    private League() { }
#pragma warning restore CS8618
}
