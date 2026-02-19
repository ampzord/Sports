namespace Sports.Api.Features.Teams.GetTeams;

using ErrorOr;
using MediatR;
using Sports.Api.Features.Teams._Shared;
using System.Collections.Immutable;

public record GetTeamsQuery(Guid? LeagueId) : IRequest<ErrorOr<ImmutableList<TeamResponse>>>;
