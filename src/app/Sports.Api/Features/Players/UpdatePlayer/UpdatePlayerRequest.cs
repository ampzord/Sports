namespace Sports.Api.Features.Players.UpdatePlayer;

using Microsoft.AspNetCore.Mvc;

public record UpdatePlayerRequest(
    [FromRoute] Guid Id,
    string Name,
    string Position,
    Guid? TeamId = null)
{
    public static UpdatePlayerRequest Example => new(Guid.Empty, "Lionel Messi", "RW", Guid.Empty);
}
