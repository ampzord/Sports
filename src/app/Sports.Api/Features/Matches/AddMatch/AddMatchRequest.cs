namespace Sports.Api.Features.Matches.AddMatch;

public record AddMatchRequest(
    Guid LeagueId,
    Guid HomeTeamId,
    Guid AwayTeamId,
    int? TotalPasses = null)
{
    public static AddMatchRequest Example => new(Guid.Empty, Guid.Empty, Guid.Empty);
}