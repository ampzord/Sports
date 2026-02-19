namespace Sports.Domain.MatchAggregate.ValueObjects;

using Sports.Domain.Common.Models;

public sealed class MatchId : ValueObject
{
    public Guid Value { get; }

    private MatchId(Guid value) => Value = value;

    public static MatchId CreateUnique() => new(Guid.CreateVersion7());

    public static MatchId Create(Guid value) => new(value);

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
