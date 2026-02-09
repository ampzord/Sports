namespace Sports.Api.Features.Matches.DeleteMatch;

using ErrorOr;
using MediatR;
using Sports.Api.Database;

public class DeleteMatchHandler(SportsDbContext db)
    : IRequestHandler<DeleteMatchCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(
        DeleteMatchCommand command,
        CancellationToken cancellationToken)
    {
        var match = await db.Matches.FindAsync([command.Id], cancellationToken);

        if (match is null)
            return Error.NotFound("Match.NotFound", "Match not found");

        db.Matches.Remove(match);
        await db.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}