namespace Sports.Api.Features.Leagues._Shared;

public record LeagueResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
