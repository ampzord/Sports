using Microsoft.AspNetCore.Mvc;

namespace Sports.Api.Features.Matches.DeleteMatch;

public record DeleteMatchRequest([FromRoute] int Id)
{
    public static DeleteMatchRequest Example => new(1);
}