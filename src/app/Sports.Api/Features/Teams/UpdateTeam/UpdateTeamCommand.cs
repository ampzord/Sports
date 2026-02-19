namespace Sports.Api.Features.Teams.UpdateTeam;

using ErrorOr;
using MediatR;
using Sports.Api.Features.Teams._Shared;

public record UpdateTeamCommand(
    Guid Id,
    string Name,
    Guid? LeagueId
) : IRequest<ErrorOr<TeamResponse>>;
