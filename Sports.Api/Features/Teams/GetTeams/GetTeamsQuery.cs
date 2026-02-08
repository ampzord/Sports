namespace Sports.Api.Features.Teams.GetTeams;

using Sports.Api.Features.Teams._Shared.Responses;

using ErrorOr;
using MediatR;

public record GetTeamsQuery(int? LeagueId) : IRequest<ErrorOr<List<TeamResponse>>>;
