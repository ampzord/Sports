namespace Sports.Api.Features.Players.AddPlayer;

using Sports.Api.Features.Players._Shared.Responses;

using ErrorOr;
using MediatR;
using Sports.Shared.Entities;

public record AddPlayerCommand(
    string Name,
    PlayerPosition Position,
    int TeamId
) : IRequest<ErrorOr<PlayerResponse>>;