using Microsoft.AspNetCore.Mvc;

namespace Sports.Api.Features.Players.GetPlayerById;

public record GetPlayerByIdRequest([FromRoute] int Id);
