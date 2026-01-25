namespace Sports.Api.Features.Players.AddPlayer;

using MediatR;
using Sports.Api.Entities;

public record AddPlayerCommand(
    string Name,
    PlayerPosition Position,
    int? TeamId
) : IRequest<AddPlayerResponse>;