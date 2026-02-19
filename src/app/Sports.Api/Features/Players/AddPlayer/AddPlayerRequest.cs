namespace Sports.Api.Features.Players.AddPlayer;

using Sports.Domain.Entities;

public record AddPlayerRequest(
    Guid TeamId,
    string Name,
    PlayerPosition Position)
{
    public static AddPlayerRequest Example =>
        new(Guid.Empty, "Lionel Messi", PlayerPosition.RW);
}
