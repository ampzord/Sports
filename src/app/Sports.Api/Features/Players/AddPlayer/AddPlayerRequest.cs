namespace Sports.Api.Features.Players.AddPlayer;

using Sports.Domain.Entities;

public record AddPlayerRequest(
    int TeamId,
    string Name,
    PlayerPosition Position)
{
    public static AddPlayerRequest Example =>
        new(1, "Lionel Messi", PlayerPosition.RW);
}
