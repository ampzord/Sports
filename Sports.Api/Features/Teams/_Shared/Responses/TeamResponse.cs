namespace Sports.Api.Features.Teams._Shared.Responses;

public record TeamResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int LeagueId { get; init; }
}
