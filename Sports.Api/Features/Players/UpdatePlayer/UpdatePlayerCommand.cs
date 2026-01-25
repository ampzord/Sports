namespace Sports.Api.Features.Players.UpdatePlayer;

using MediatR;
using Sports.Api.Entities;

public record UpdatePlayerCommand(
    int Id,
    string Name,
    PlayerPosition Position,
    int? TeamId
) : IRequest<UpdatePlayerResponse?>;