namespace Sports.Api.Features.Players.GetPlayers;

using MediatR;

public record GetPlayersQuery : IRequest<List<GetPlayersResponse>>;