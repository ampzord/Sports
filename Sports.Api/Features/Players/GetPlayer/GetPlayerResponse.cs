namespace Sports.Api.Features.Players.GetPlayer;

using Sports.Api.Entities;

public class GetPlayerResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public PlayerPosition Position { get; set; }
    public int? TeamId { get; set; }
}