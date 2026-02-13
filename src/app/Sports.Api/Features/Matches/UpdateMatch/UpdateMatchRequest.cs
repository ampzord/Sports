using Microsoft.AspNetCore.Mvc;

namespace Sports.Api.Features.Matches.UpdateMatch;

public record UpdateMatchRequest(
    [FromRoute] int Id,
    int HomeTeamId,
    int AwayTeamId,
    int? TotalPasses = null)
{
    public static UpdateMatchRequest Example => new(1, 1, 2, 500);
}