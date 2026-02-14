namespace Sports.Api.Features.Leagues.GetLeagueById;

using Microsoft.AspNetCore.Mvc;

public record GetLeagueByIdRequest([FromRoute] int Id)
{
    public static GetLeagueByIdRequest Example => new(1);
}
