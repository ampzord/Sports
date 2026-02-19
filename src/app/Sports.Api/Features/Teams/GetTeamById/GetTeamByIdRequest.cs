namespace Sports.Api.Features.Teams.GetTeamById;

using Microsoft.AspNetCore.Mvc;

public record GetTeamByIdRequest([FromRoute] Guid Id);
