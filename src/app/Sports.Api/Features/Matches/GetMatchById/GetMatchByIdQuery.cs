using Sports.Api.Features.Matches._Shared;

namespace Sports.Api.Features.Matches.GetMatchById;


using ErrorOr;
using MediatR;

public record GetMatchByIdQuery(int Id) : IRequest<ErrorOr<MatchResponse>>;