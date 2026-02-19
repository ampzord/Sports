namespace Sports.Api.Features.Leagues.GetLeagueById;

using Microsoft.AspNetCore.Mvc;

public record GetLeagueByIdRequest([FromRoute] Guid Id)
{
    public static GetLeagueByIdRequest Example => new(Guid.Empty);
}
