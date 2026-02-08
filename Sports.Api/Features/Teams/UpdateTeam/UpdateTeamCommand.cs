namespace Sports.Api.Features.Teams.UpdateTeam;

using Sports.Api.Features.Teams._Shared.Responses;

using ErrorOr;
using MediatR;

public record UpdateTeamCommand(
    int Id,
    string Name,
    int? LeagueId
) : IRequest<ErrorOr<TeamResponse>>;