namespace Sports.Api.Features.Leagues.GetLeagues;

using Sports.Api.Features.Leagues._Shared.Responses;

using MediatR;

public record GetLeaguesQuery : IRequest<List<LeagueResponse>>;