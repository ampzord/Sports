namespace Sports.Api.Features.Leagues.UpdateLeague;

using Sports.Api.Features.Leagues._Shared.Responses;

using ErrorOr;
using MediatR;

public record UpdateLeagueCommand(
    int Id,
    string Name
) : IRequest<ErrorOr<LeagueResponse>>;