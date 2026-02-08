using Sports.Shared.Entities;

namespace Sports.Api.Features.Players.AddPlayer;

public record AddPlayerRequest(
    int TeamId,
    string Name,
    PlayerPosition Position)
{
    public static AddPlayerRequest Example =>
        new(1, "Lionel Messi", PlayerPosition.RW);
}