namespace Sports.Api.Features.Players._Shared;

using Sports.Domain.PlayerAggregate.Enums;

public record PlayerResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public PlayerPosition Position { get; init; }
    public Guid TeamId { get; init; }
}
