namespace Sports.Api.Features.Players.UpdatePlayer;

using Microsoft.AspNetCore.Mvc;

public record UpdatePlayerRequest(
    [FromRoute] int Id,
    string Name,
    string Position,
    int? TeamId = null)
{
    public static UpdatePlayerRequest Example => new(1, "Lionel Messi", "RW", 1);
}
