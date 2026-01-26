namespace Sports.Api.Features.Matches.GetMatch;

using MediatR;

public record GetMatchQuery(int Id) : IRequest<GetMatchResponse?>;