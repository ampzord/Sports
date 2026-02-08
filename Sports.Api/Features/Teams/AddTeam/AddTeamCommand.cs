namespace Sports.Api.Features.Teams.AddTeam;

using Sports.Api.Features.Teams._Shared.Responses;

using ErrorOr;
using MediatR;

public record AddTeamCommand(
    string Name,
    int LeagueId
) : IRequest<ErrorOr<TeamResponse>>;