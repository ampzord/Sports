using Sports.Api.Features.Leagues._Shared;

namespace Sports.Api.Features.Leagues.AddLeague;


using ErrorOr;
using MediatR;

public record AddLeagueCommand(
    string Name
) : IRequest<ErrorOr<LeagueResponse>>;