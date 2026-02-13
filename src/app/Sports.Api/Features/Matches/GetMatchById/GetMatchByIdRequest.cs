using Microsoft.AspNetCore.Mvc;

namespace Sports.Api.Features.Matches.GetMatchById;

public record GetMatchByIdRequest([FromRoute] int Id);
