namespace Sports.Domain.Common.Models;

using System.Collections.Immutable;

public interface IHasDomainEvents
{
    ImmutableList<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
