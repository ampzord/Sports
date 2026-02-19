namespace Sports.Api.Features.Teams._Shared;

public record TeamResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid LeagueId { get; init; }
}
