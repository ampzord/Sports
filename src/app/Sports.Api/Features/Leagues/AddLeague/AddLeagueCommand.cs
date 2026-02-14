namespace Sports.Api.Features.Leagues.AddLeague;

using ErrorOr;
using MediatR;
using Sports.Api.Features.Leagues._Shared;

public record AddLeagueCommand(
    string Name
) : IRequest<ErrorOr<LeagueResponse>>;
