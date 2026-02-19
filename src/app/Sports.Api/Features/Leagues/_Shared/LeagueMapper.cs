namespace Sports.Api.Features.Leagues._Shared;

using System.Collections.Immutable;
using Riok.Mapperly.Abstractions;
using Sports.Api.Features.Leagues.AddLeague;
using Sports.Api.Features.Leagues.DeleteLeague;
using Sports.Api.Features.Leagues.UpdateLeague;
using Sports.Domain.LeagueAggregate;
using Sports.Domain.LeagueAggregate.ValueObjects;

[Mapper]
public partial class LeagueMapper
{
    [MapperIgnoreSource(nameof(League.Teams))]
    [MapperIgnoreSource(nameof(League.DomainEvents))]
    public partial LeagueResponse ToResponse(League league);

    public partial ImmutableList<LeagueResponse> ToResponseList(List<League> leagues);

    public partial AddLeagueCommand ToCommand(AddLeagueRequest request);

    public partial UpdateLeagueCommand ToCommand(UpdateLeagueRequest request);

    public partial DeleteLeagueCommand ToCommand(DeleteLeagueRequest request);

    private static Guid ToGuid(LeagueId id) => id.Value;
}
