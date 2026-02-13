namespace Sports.Api.Features.Matches.AddMatch;

public record AddMatchRequest(
    int LeagueId,
    int HomeTeamId,
    int AwayTeamId,
    int? TotalPasses = null)
{
    public static AddMatchRequest Example => new(1, 1, 2);
}