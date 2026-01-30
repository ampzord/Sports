using Microsoft.AspNetCore.Mvc;

namespace Sports.Api.Features.Players.UpdatePlayer;

public class UpdatePlayerRequest
{
    [FromRoute]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public int? TeamId { get; set; }
}