namespace Sports.Api.Features.Players.UpdatePlayer;

using ErrorOr;
using MediatR;
using Sports.Api.Features.Players._Shared;
using Sports.Domain.Entities;

public record UpdatePlayerCommand(
    int Id,
    string Name,
    PlayerPosition Position,
    int TeamId
) : IRequest<ErrorOr<PlayerResponse>>;
