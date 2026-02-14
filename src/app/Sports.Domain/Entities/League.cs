namespace Sports.Domain.Entities;

public class League
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Team> Teams { get; set; } = [];
}
