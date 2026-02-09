using Sports.Api.Features.Players._Shared;

namespace Sports.Api.Features.Players.AddPlayer;


using ErrorOr;
using MediatR;
using Sports.Shared.Entities;

public record AddPlayerCommand(
    string Name,
    PlayerPosition Position,
    int TeamId
) : IRequest<ErrorOr<PlayerResponse>>;