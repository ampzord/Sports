namespace Sports.Api.Features.Teams.GetTeam;

using MediatR;

public record GetTeamQuery(int Id) : IRequest<GetTeamResponse?>;