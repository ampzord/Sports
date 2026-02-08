namespace Sports.Api.Features.Leagues.AddLeague;

using Sports.Api.Features.Leagues._Shared.Responses;

using ErrorOr;
using MediatR;

public record AddLeagueCommand(
    string Name
) : IRequest<ErrorOr<LeagueResponse>>;