namespace Sports.Api.Features.Teams.AddTeam;

using ErrorOr;
using MediatR;
using Sports.Api.Features.Teams._Shared;

public record AddTeamCommand(
    string Name,
    int LeagueId
) : IRequest<ErrorOr<TeamResponse>>;
