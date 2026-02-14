namespace Sports.Api.Features.Matches.GetMatchById;

using ErrorOr;
using MediatR;
using Sports.Api.Features.Matches._Shared;

public record GetMatchByIdQuery(int Id) : IRequest<ErrorOr<MatchResponse>>;
