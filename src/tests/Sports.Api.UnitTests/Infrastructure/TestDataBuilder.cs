namespace Sports.Api.UnitTests.Infrastructure;

using Sports.Api.Database;
using Sports.Domain.Entities;

public class TestDataBuilder(SportsDbContext db)
{
    public TestDataBuilder WithLeague(int id = 1, string name = "Premier League")
    {
        db.Leagues.Add(new League { Id = id, Name = name });
        return this;
    }

    public TestDataBuilder WithTeam(int id = 1, string name = "Arsenal", int leagueId = 1)
    {
        db.Teams.Add(new Team { Id = id, Name = name, LeagueId = leagueId });
        return this;
    }

    public TestDataBuilder WithPlayer(int id = 1, string name = "Saka",
        PlayerPosition position = PlayerPosition.RW, int teamId = 1)
    {
        db.Players.Add(new Player { Id = id, Name = name, Position = position, TeamId = teamId });
        return this;
    }

    public TestDataBuilder WithMatch(int id = 1, int homeTeamId = 1, int awayTeamId = 2,
        int? totalPasses = null)
    {
        db.Matches.Add(new Match { Id = id, HomeTeamId = homeTeamId, AwayTeamId = awayTeamId, TotalPasses = totalPasses });
        return this;
    }

    public async Task<TestDataBuilder> SaveAsync()
    {
        await db.SaveChangesAsync();
        return this;
    }
}
