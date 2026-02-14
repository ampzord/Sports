namespace Sports.Api.Features.Matches.DeleteMatch;

using Microsoft.AspNetCore.Mvc;

public record DeleteMatchRequest([FromRoute] int Id)
{
    public static DeleteMatchRequest Example => new(1);
}
