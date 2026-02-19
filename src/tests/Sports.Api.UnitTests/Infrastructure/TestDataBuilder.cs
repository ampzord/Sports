namespace Sports.Api.UnitTests.Infrastructure;

using Sports.Api.Database;
using Sports.Domain.Entities;

public class TestDataBuilder(SportsDbContext db)
{
    private static Guid TestGuid(int n) => new($"00000000-0000-0000-0000-{n:D12}");

    public TestDataBuilder WithLeague(int id = 1, string name = "Premier League")
    {
        db.Leagues.Add(League.Create(TestGuid(id), name));
        return this;
    }

    public TestDataBuilder WithTeam(int id = 1, string name = "Arsenal", int leagueId = 1)
    {
        db.Teams.Add(Team.Create(TestGuid(id), name, TestGuid(leagueId)));
        return this;
    }

    public TestDataBuilder WithPlayer(int id = 1, string name = "Saka",
        PlayerPosition position = PlayerPosition.RW, int teamId = 1)
    {
        db.Players.Add(Player.Create(TestGuid(id), name, position, TestGuid(teamId)));
        return this;
    }

    public TestDataBuilder WithMatch(int id = 1, int homeTeamId = 1, int awayTeamId = 2,
        int? totalPasses = null)
    {
        db.Matches.Add(Match.Create(TestGuid(id), TestGuid(homeTeamId), TestGuid(awayTeamId), totalPasses));
        return this;
    }

    public async Task<TestDataBuilder> SaveAsync()
    {
        await db.SaveChangesAsync();
        return this;
    }

    public static Guid Id(int n) => TestGuid(n);
}
