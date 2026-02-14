namespace Sports.Api.Features.Players.GetPlayers;

using ErrorOr;
using MediatR;
using Sports.Api.Features.Players._Shared;
using System.Collections.Immutable;

public record GetPlayersQuery(int? TeamId) : IRequest<ErrorOr<ImmutableList<PlayerResponse>>>;
