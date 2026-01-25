namespace Sports.Api.Features.Players.AddPlayer;

public class AddPlayerResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public int? TeamId { get; set; }
}
