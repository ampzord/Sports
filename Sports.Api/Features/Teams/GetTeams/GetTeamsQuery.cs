using Sports.Api.Features.Teams._Shared;

namespace Sports.Api.Features.Teams.GetTeams;


using ErrorOr;
using MediatR;

public record GetTeamsQuery(int? LeagueId) : IRequest<ErrorOr<List<TeamResponse>>>;
