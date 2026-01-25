namespace Sports.Api.Entities;

public class Match
{
    public int Id { get; set; }
    public int HomeTeamId { get; set; }
    public int AwayTeamId { get; set; }
    public int? TotalPasses { get; set; }
}