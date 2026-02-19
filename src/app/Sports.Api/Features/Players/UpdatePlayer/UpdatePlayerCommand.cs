namespace Sports.Api.Features.Players.UpdatePlayer;

using ErrorOr;
using MediatR;
using Sports.Api.Features.Players._Shared;
using Sports.Domain.Entities;

public record UpdatePlayerCommand(
    Guid Id,
    string Name,
    PlayerPosition Position,
    Guid TeamId
) : IRequest<ErrorOr<PlayerResponse>>;
