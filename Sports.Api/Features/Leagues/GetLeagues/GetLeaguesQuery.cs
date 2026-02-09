using Sports.Api.Features.Leagues._Shared;

namespace Sports.Api.Features.Leagues.GetLeagues;


using MediatR;

public record GetLeaguesQuery : IRequest<List<LeagueResponse>>;