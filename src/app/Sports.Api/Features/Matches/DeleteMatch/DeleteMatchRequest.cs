namespace Sports.Api.Features.Matches.DeleteMatch;

using Microsoft.AspNetCore.Mvc;

public record DeleteMatchRequest([FromRoute] Guid Id)
{
    public static DeleteMatchRequest Example => new(Guid.Empty);
}
