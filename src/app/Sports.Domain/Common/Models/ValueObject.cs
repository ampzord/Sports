namespace Sports.Domain.Common.Models;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        var valueObject = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
    }

    public static bool operator ==(ValueObject left, ValueObject right) => Equals(left, right);

    public static bool operator !=(ValueObject left, ValueObject right) => !Equals(left, right);

    public override int GetHashCode() => GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y); // XOR to combine hash codes

    public bool Equals(ValueObject? other) => Equals((object?)other);
}
