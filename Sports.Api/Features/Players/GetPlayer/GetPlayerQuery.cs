namespace Sports.Api.Features.Players.GetPlayer;

using MediatR;

public record GetPlayerQuery(int Id) : IRequest<GetPlayerResponse?>;