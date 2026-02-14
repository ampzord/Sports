namespace Sports.Api.Features.Players.GetPlayerById;

using ErrorOr;
using MediatR;
using Sports.Api.Features.Players._Shared;

public record GetPlayerByIdQuery(int Id) : IRequest<ErrorOr<PlayerResponse>>;
