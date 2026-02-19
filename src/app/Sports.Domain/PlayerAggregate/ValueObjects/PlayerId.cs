namespace Sports.Domain.PlayerAggregate.ValueObjects;

using Sports.Domain.Common.Models;

public sealed class PlayerId : ValueObject
{
    public Guid Value { get; }

    private PlayerId(Guid value) => Value = value;

    public static PlayerId CreateUnique() => new(Guid.CreateVersion7());

    public static PlayerId Create(Guid value) => new(value);

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
