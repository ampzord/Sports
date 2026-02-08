namespace Sports.Api.Features.Leagues._Shared.Responses;

public record LeagueResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
