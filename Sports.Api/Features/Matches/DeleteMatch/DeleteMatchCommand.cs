namespace Sports.Api.Features.Matches.DeleteMatch;

using MediatR;

public record DeleteMatchCommand(int Id) : IRequest<DeleteMatchResponse>;