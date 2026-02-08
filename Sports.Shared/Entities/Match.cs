namespace Sports.Shared.Entities;

public class Match
{
    public int Id { get; set; }
    public int HomeTeamId { get; set; }
    public Team HomeTeam { get; set; } = null!;
    public int AwayTeamId { get; set; }
    public Team AwayTeam { get; set; } = null!;
    public int? TotalPasses { get; set; }
}
