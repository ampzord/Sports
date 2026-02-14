namespace Sports.Api.Features.Matches.UpdateMatch;

using Microsoft.AspNetCore.Mvc;

public record UpdateMatchRequest(
    [FromRoute] int Id,
    int HomeTeamId,
    int AwayTeamId,
    int? TotalPasses = null)
{
    public static UpdateMatchRequest Example => new(1, 1, 2, 500);
}
