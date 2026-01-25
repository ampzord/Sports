namespace Sports.Api.Features.Teams.AddTeam;

using MediatR;

public record AddTeamCommand(
    string Name,
    int? LeagueId
) : IRequest<AddTeamResponse>;