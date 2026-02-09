using Sports.Api.Features.Players._Shared;

namespace Sports.Api.Features.Players.GetPlayerById;


using ErrorOr;
using MediatR;

public record GetPlayerByIdQuery(int Id) : IRequest<ErrorOr<PlayerResponse>>;