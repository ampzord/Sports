using FastEndpoints;

namespace Sports.Api.Features.Players.GetPlayers;

public record GetPlayersRequest([property: QueryParam] int? TeamId);
