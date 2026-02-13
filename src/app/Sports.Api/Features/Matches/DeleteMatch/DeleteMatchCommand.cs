namespace Sports.Api.Features.Matches.DeleteMatch;

using ErrorOr;
using MediatR;

public record DeleteMatchCommand(int Id) : IRequest<ErrorOr<Deleted>>;