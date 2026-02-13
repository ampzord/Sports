namespace Sports.Api.Features.Matches.SimulateMatches;

using ErrorOr;
using MediatR;

public record SimulateMatchesCommand : IRequest<ErrorOr<SimulateMatchesResponse>>;
