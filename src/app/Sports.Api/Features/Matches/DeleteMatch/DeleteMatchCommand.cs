namespace Sports.Api.Features.Matches.DeleteMatch;

using ErrorOr;
using MediatR;

public record DeleteMatchCommand(Guid Id) : IRequest<ErrorOr<Deleted>>;