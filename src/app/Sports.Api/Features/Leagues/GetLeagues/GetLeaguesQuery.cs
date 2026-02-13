using System.Collections.Immutable;
using Sports.Api.Features.Leagues._Shared;

namespace Sports.Api.Features.Leagues.GetLeagues;


using MediatR;

public record GetLeaguesQuery : IRequest<ImmutableList<LeagueResponse>>;