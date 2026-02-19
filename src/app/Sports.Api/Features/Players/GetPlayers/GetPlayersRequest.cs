namespace Sports.Api.Features.Players.GetPlayers;

using FastEndpoints;

public record GetPlayersRequest([property: QueryParam] Guid? TeamId);
