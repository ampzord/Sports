namespace Sports.Api.Features.Players.GetPlayerById;

using Sports.Api.Features.Players._Shared.Responses;

using ErrorOr;
using MediatR;

public record GetPlayerByIdQuery(int Id) : IRequest<ErrorOr<PlayerResponse>>;