namespace Sports.Api.Features.Matches.GetMatchById;

using Sports.Api.Features.Matches._Shared.Responses;

using ErrorOr;
using MediatR;
using Sports.Api.Database;

public class GetMatchByIdHandler(SportsDbContext db, GetMatchByIdMapper mapper)
    : IRequestHandler<GetMatchByIdQuery, ErrorOr<MatchResponse>>
{
    public async Task<ErrorOr<MatchResponse>> Handle(
        GetMatchByIdQuery query,
        CancellationToken cancellationToken)
    {
        var match = await db.Matches.FindAsync(query.Id, cancellationToken);

        if (match is null)
            return Error.NotFound("Match.NotFound", "Match not found");

        return mapper.ToResponse(match);
    }
}