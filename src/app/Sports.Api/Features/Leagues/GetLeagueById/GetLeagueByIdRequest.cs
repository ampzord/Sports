using Microsoft.AspNetCore.Mvc;

namespace Sports.Api.Features.Leagues.GetLeagueById;

public record GetLeagueByIdRequest([FromRoute] int Id)
{
    public static GetLeagueByIdRequest Example => new(1);
}