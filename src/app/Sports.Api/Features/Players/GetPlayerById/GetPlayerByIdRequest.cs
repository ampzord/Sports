namespace Sports.Api.Features.Players.GetPlayerById;

using Microsoft.AspNetCore.Mvc;

public record GetPlayerByIdRequest([FromRoute] Guid Id);
