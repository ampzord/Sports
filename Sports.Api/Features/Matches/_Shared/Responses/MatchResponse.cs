namespace Sports.Api.Features.Matches._Shared.Responses;

public record MatchResponse
{
    public int Id { get; init; }
    public int HomeTeamId { get; init; }
    public int AwayTeamId { get; init; }
    public int? TotalPasses { get; init; }
}
