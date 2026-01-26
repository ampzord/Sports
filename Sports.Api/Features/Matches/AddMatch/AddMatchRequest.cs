namespace Sports.Api.Features.Matches.AddMatch;

public class AddMatchRequest
{
    public int HomeTeamId { get; set; }
    public int AwayTeamId { get; set; }
    public int? TotalPasses { get; set; }
}