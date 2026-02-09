using Sports.Api.Features.Teams._Shared;

namespace Sports.Api.Features.Teams.UpdateTeam;


using ErrorOr;
using MediatR;

public record UpdateTeamCommand(
    int Id,
    string Name,
    int? LeagueId
) : IRequest<ErrorOr<TeamResponse>>;