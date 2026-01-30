using Microsoft.AspNetCore.Mvc;

namespace Sports.Api.Features.Matches.UpdateMatch;

public class UpdateMatchRequest
{
    [FromRoute]
    public int Id { get; set; }
    public int HomeTeamId { get; set; }
    public int AwayTeamId { get; set; }
    public int? TotalPasses { get; set; }
}