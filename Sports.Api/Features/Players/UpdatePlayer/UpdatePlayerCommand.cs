namespace Sports.Api.Features.Players.UpdatePlayer;

using Sports.Api.Features.Players._Shared.Responses;

using ErrorOr;
using MediatR;
using Sports.Shared.Entities;

public record UpdatePlayerCommand(
    int Id,
    string Name,
    PlayerPosition Position,
    int TeamId
) : IRequest<ErrorOr<PlayerResponse>>;