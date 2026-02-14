namespace Sports.Api.Features.Teams.UpdateTeam;

using ErrorOr;
using MediatR;
using Sports.Api.Features.Teams._Shared;

public record UpdateTeamCommand(
    int Id,
    string Name,
    int? LeagueId
) : IRequest<ErrorOr<TeamResponse>>;
