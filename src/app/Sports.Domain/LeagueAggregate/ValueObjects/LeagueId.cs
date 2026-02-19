namespace Sports.Domain.LeagueAggregate.ValueObjects;

using Sports.Domain.Common.Models;

public sealed class LeagueId : ValueObject
{
    public Guid Value { get; }

    private LeagueId(Guid value) => Value = value;

    public static LeagueId CreateUnique() => new(Guid.CreateVersion7());

    public static LeagueId Create(Guid value) => new(value);

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
