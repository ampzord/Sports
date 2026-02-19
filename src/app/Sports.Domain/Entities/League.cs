namespace Sports.Domain.Entities;

using Sports.Domain.Common.Models;

public class League : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;

    public ICollection<Team> Teams { get; set; } = [];

    public static League Create(string name) =>
        new() { Id = Guid.CreateVersion7(), Name = name };

    public static League Create(Guid id, string name) =>
        new() { Id = id, Name = name };
}
