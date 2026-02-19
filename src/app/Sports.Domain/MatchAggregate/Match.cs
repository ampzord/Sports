namespace Sports.Domain.MatchAggregate;

using Sports.Domain.Common.Models;
using Sports.Domain.MatchAggregate.ValueObjects;
using Sports.Domain.TeamAggregate;
using Sports.Domain.TeamAggregate.ValueObjects;

public class Match : Entity<MatchId>
{
    private Match(MatchId id, TeamId homeTeamId, TeamId awayTeamId, int? totalPasses) : base(id)
    {
        HomeTeamId = homeTeamId;
        AwayTeamId = awayTeamId;
        TotalPasses = totalPasses;
    }

    public TeamId HomeTeamId { get; private set; }

    public Team HomeTeam { get; private set; } = null!;

    public TeamId AwayTeamId { get; private set; }

    public Team AwayTeam { get; private set; } = null!;

    public int? TotalPasses { get; private set; }

    public static Match Create(TeamId homeTeamId, TeamId awayTeamId, int? totalPasses = null) =>
        new(MatchId.CreateUnique(), homeTeamId, awayTeamId, totalPasses);

    public void Update(TeamId homeTeamId, TeamId awayTeamId, int? totalPasses)
    {
        HomeTeamId = homeTeamId;
        AwayTeamId = awayTeamId;
        TotalPasses = totalPasses;
    }

    public void SetTotalPasses(int totalPasses) => TotalPasses = totalPasses;

#pragma warning disable CS8618
    private Match() { }
#pragma warning restore CS8618
}
