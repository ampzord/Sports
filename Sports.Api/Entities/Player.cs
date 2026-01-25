namespace Sports.Api.Entities;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public PlayerPosition Position { get; set; }
    public int? TeamId { get; set; }
}
