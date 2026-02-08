namespace Sports.Api.Features.Matches.UpdateMatch;

using Sports.Api.Features.Matches._Shared.Responses;

using ErrorOr;
using MediatR;
using Sports.Api.Database;

public class UpdateMatchHandler(SportsDbContext db, UpdateMatchMapper mapper)
    : IRequestHandler<UpdateMatchCommand, ErrorOr<MatchResponse>>
{
    public async Task<ErrorOr<MatchResponse>> Handle(
        UpdateMatchCommand command,
        CancellationToken cancellationToken)
    {
        var match = await db.Matches.FindAsync(command.Id, cancellationToken);

        if (match is null)
            return Error.NotFound("Match.NotFound", "Match not found");

        mapper.Apply(command, match);
        await db.SaveChangesAsync(cancellationToken);

        return mapper.ToResponse(match);
    }
}