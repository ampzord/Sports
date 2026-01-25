namespace Sports.Api.Features.Teams.UpdateTeam;

using MediatR;

public record UpdateTeamCommand(
    int Id,
    string Name,
    int? LeagueId
) : IRequest<UpdateTeamResponse?>;