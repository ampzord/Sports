namespace Sports.Api.Features.Players.AddPlayer;

using ErrorOr;
using MediatR;
using Sports.Api.Features.Players._Shared;
using Sports.Domain.PlayerAggregate.Enums;

public record AddPlayerCommand(
    string Name,
    PlayerPosition Position,
    Guid TeamId
) : IRequest<ErrorOr<PlayerResponse>>;
