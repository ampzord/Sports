namespace Sports.Api.Features.Leagues.GetLeagueById;

using Sports.Api.Features.Leagues._Shared.Responses;

using ErrorOr;
using MediatR;

public record GetLeagueByIdQuery(int Id) : IRequest<ErrorOr<LeagueResponse>>;