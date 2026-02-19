namespace Sports.Api.UnitTests.Infrastructure;

using Sports.Api.Database;
using Sports.Domain.LeagueAggregate;
using Sports.Domain.MatchAggregate;
using Sports.Domain.MatchAggregate.ValueObjects;
using Sports.Domain.PlayerAggregate;
using Sports.Domain.PlayerAggregate.Enums;
using Sports.Domain.PlayerAggregate.ValueObjects;
using Sports.Domain.TeamAggregate;
using Sports.Domain.TeamAggregate.ValueObjects;
using Sports.Domain.LeagueAggregate.ValueObjects;

public class TestDataBuilder(SportsDbContext db)
{
    private readonly Dictionary<string, Guid> _ids = [];

    public TestDataBuilder WithLeague(string name = "Premier League")
    {
        var league = League.Create(name);
        _ids[name] = league.Id.Value;
        db.Leagues.Add(league);
        return this;
    }

    public TestDataBuilder WithTeam(string name = "Arsenal", string inLeague = "Premier League")
    {
        var team = Team.Create(name, LeagueId.Create(_ids[inLeague]));
        _ids[name] = team.Id.Value;
        db.Teams.Add(team);
        return this;
    }

    public TestDataBuilder WithPlayer(string name = "Saka",
        PlayerPosition position = PlayerPosition.RW, string inTeam = "Arsenal")
    {
        var player = Player.Create(name, position, TeamId.Create(_ids[inTeam]));
        _ids[name] = player.Id.Value;
        db.Players.Add(player);
        return this;
    }

    public TestDataBuilder WithMatch(string key = "match", string homeTeam = "Arsenal",
        string awayTeam = "Chelsea", int? totalPasses = null)
    {
        var match = Match.Create(TeamId.Create(_ids[homeTeam]), TeamId.Create(_ids[awayTeam]), totalPasses);
        _ids[key] = match.Id.Value;
        db.Matches.Add(match);
        return this;
    }

    public async Task<TestDataBuilder> SaveAsync()
    {
        await db.SaveChangesAsync();
        return this;
    }

    public Guid Id(string name) => _ids[name];
}
