namespace Sports.Api.Features.Players.AddPlayer;

public class AddPlayerRequest
{
    public string Name { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public int? TeamId { get; set; }
}