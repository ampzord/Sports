namespace Sports.Api.Features.Players.AddPlayer;

using Sports.Api.Entities;

public class AddPlayerResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public PlayerPosition Position { get; set; }
    public int? TeamId { get; set; }
}