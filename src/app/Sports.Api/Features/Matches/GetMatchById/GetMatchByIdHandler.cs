namespace Sports.Api.Features.Matches.GetMatchById;

using Sports.Api.Features.Matches._Shared;
using ErrorOr;
using MediatR;
using Sports.Api.Database;
using Sports.Domain.MatchAggregate.ValueObjects;

public class GetMatchByIdHandler(SportsDbContext db, MatchMapper mapper)
    : IRequestHandler<GetMatchByIdQuery, ErrorOr<MatchResponse>>
{
    public async Task<ErrorOr<MatchResponse>> Handle(
        GetMatchByIdQuery query,
        CancellationToken cancellationToken)
    {
        var match = await db.Matches.FindAsync([MatchId.Create(query.Id)], cancellationToken);

        if (match is null)
            return MatchErrors.NotFound;

        return mapper.ToResponse(match);
    }
}