using Sports.Api.Features.Players._Shared;

namespace Sports.Api.Features.Players.GetPlayers;


using ErrorOr;
using MediatR;

public record GetPlayersQuery(int? TeamId) : IRequest<ErrorOr<List<PlayerResponse>>>;
