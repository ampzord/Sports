namespace Sports.Api.Features.Matches.SimulateMatches;

public record SimulateMatchesResponse
{
    public string Message { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
}
