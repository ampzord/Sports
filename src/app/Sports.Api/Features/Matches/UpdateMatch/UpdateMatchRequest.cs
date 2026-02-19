namespace Sports.Api.Features.Matches.UpdateMatch;

using Microsoft.AspNetCore.Mvc;

public record UpdateMatchRequest(
    [FromRoute] Guid Id,
    Guid HomeTeamId,
    Guid AwayTeamId,
    int? TotalPasses = null)
{
    public static UpdateMatchRequest Example => new(Guid.Empty, Guid.Empty, Guid.Empty, 500);
}
