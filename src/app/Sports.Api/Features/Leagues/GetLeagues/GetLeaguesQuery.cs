namespace Sports.Api.Features.Leagues.GetLeagues;

using MediatR;
using Sports.Api.Features.Leagues._Shared;
using System.Collections.Immutable;

public record GetLeaguesQuery : IRequest<ImmutableList<LeagueResponse>>;
