namespace Sports.Domain.Entities;

using Sports.Domain.Common.Models;

public class Match : Entity<Guid>
{
    public Guid HomeTeamId { get; set; }

    public Team HomeTeam { get; set; } = null!;

    public Guid AwayTeamId { get; set; }

    public Team AwayTeam { get; set; } = null!;

    public int? TotalPasses { get; set; }

    public static Match Create(Guid homeTeamId, Guid awayTeamId, int? totalPasses = null) =>
        new() { Id = Guid.CreateVersion7(), HomeTeamId = homeTeamId, AwayTeamId = awayTeamId, TotalPasses = totalPasses };

    public static Match Create(Guid id, Guid homeTeamId, Guid awayTeamId, int? totalPasses = null) =>
        new() { Id = id, HomeTeamId = homeTeamId, AwayTeamId = awayTeamId, TotalPasses = totalPasses };
}
