namespace Sports.Api.Entities;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? LeagueId { get; set; }
    public ICollection<Player> Players { get; set; } = [];
}