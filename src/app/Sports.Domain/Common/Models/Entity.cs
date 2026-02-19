namespace Sports.Domain.Common.Models;

using System.Collections.Immutable;

public abstract class Entity<TId> : IEquatable<Entity<TId>>, IHasDomainEvents
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public TId Id { get; protected set; }

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    ImmutableList<IDomainEvent> IHasDomainEvents.DomainEvents => throw new NotImplementedException();

    protected Entity(TId id) => Id = id;

    public override bool Equals(object? obj) => obj is Entity<TId> entity && Id.Equals(entity.Id);

    public bool Equals(Entity<TId>? other) => Equals((object?)other);

    public static bool operator ==(Entity<TId> left, Entity<TId> right) => Equals(left, right);

    public static bool operator !=(Entity<TId> left, Entity<TId> right) => !Equals(left, right);

    public override int GetHashCode() => Id.GetHashCode();

    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();

#pragma warning disable CS8618
    protected Entity() { }
#pragma warning restore CS8618
}
