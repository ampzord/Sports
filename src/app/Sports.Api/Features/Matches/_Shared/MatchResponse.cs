namespace Sports.Api.Features.Matches._Shared;

public record MatchResponse
{
    public Guid Id { get; init; }
    public Guid HomeTeamId { get; init; }
    public Guid AwayTeamId { get; init; }
    public int? TotalPasses { get; init; }
}
