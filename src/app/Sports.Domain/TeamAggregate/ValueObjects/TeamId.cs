namespace Sports.Domain.TeamAggregate.ValueObjects;

using Sports.Domain.Common.Models;

public sealed class TeamId : ValueObject
{
    public Guid Value { get; }

    private TeamId(Guid value) => Value = value;

    public static TeamId CreateUnique() => new(Guid.CreateVersion7());

    public static TeamId Create(Guid value) => new(value);

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
