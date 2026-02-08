namespace Sports.Api.Features.Players.GetPlayers;

using Sports.Api.Features.Players._Shared.Responses;

using ErrorOr;
using MediatR;

public record GetPlayersQuery(int? TeamId) : IRequest<ErrorOr<List<PlayerResponse>>>;
