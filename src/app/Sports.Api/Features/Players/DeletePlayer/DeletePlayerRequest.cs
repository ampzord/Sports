using Microsoft.AspNetCore.Mvc;

namespace Sports.Api.Features.Players.DeletePlayer;

public record DeletePlayerRequest([FromRoute] int Id);
