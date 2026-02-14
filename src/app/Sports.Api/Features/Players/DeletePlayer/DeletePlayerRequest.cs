namespace Sports.Api.Features.Players.DeletePlayer;

using Microsoft.AspNetCore.Mvc;

public record DeletePlayerRequest([FromRoute] int Id);
