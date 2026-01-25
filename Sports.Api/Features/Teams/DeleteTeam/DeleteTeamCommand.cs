namespace Sports.Api.Features.Teams.DeleteTeam;

using MediatR;

public record DeleteTeamCommand(int Id) : IRequest<DeleteTeamResponse>;