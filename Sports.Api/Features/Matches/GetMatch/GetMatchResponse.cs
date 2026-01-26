namespace Sports.Api.Features.Matches.GetMatch;

public class GetMatchResponse
{
    public int Id { get; set; }
    public int HomeTeamId { get; set; }
    public int AwayTeamId { get; set; }
    public int? TotalPasses { get; set; }
}