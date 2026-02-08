namespace Sports.Api.Features.Matches.GetMatchById;

using Sports.Api.Features.Matches._Shared.Responses;

using ErrorOr;
using MediatR;

public record GetMatchByIdQuery(int Id) : IRequest<ErrorOr<MatchResponse>>;