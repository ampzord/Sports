namespace Sports.Api.Features.Matches.GetMatchById;

using Microsoft.AspNetCore.Mvc;

public record GetMatchByIdRequest([FromRoute] Guid Id);
