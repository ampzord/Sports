using Microsoft.AspNetCore.Mvc;

namespace Sports.Api.Features.Teams.GetTeamById;

public record GetTeamByIdRequest([FromRoute] int Id);
