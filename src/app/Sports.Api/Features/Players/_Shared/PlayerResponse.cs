namespace Sports.Api.Features.Players._Shared;

using Sports.Domain.Entities;

public record PlayerResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public PlayerPosition Position { get; init; }
    public int TeamId { get; init; }
}
