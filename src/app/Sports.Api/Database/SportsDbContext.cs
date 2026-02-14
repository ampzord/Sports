namespace Sports.Api.Database;

using Microsoft.EntityFrameworkCore;
using Sports.Domain.Entities;
using Sports.Shared.Configurations;

public class SportsDbContext : DbContext
{
    public SportsDbContext(DbContextOptions<SportsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Player> Players => Set<Player>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<League> Leagues => Set<League>();
    public DbSet<Match> Matches => Set<Match>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PlayerConfiguration());
        modelBuilder.ApplyConfiguration(new TeamConfiguration());
        modelBuilder.ApplyConfiguration(new LeagueConfiguration());
        modelBuilder.ApplyConfiguration(new MatchConfiguration());

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<League>().HasData(
            new League { Id = 1, Name = "Premier League" },
            new League { Id = 2, Name = "La Liga" });

        modelBuilder.Entity<Team>().HasData(
            new Team { Id = 1, Name = "Arsenal", LeagueId = 1 },
            new Team { Id = 2, Name = "Manchester City", LeagueId = 1 },
            new Team { Id = 3, Name = "Liverpool", LeagueId = 1 },
            new Team { Id = 4, Name = "Chelsea", LeagueId = 1 },
            new Team { Id = 5, Name = "Real Madrid", LeagueId = 2 },
            new Team { Id = 6, Name = "Barcelona", LeagueId = 2 },
            new Team { Id = 7, Name = "Atletico Madrid", LeagueId = 2 },
            new Team { Id = 8, Name = "Sevilla", LeagueId = 2 });

        modelBuilder.Entity<Player>().HasData(
            // Arsenal (4-3-3)
            new Player { Id = 1, Name = "David Raya", Position = PlayerPosition.GK, TeamId = 1 },
            new Player { Id = 2, Name = "Ben White", Position = PlayerPosition.RB, TeamId = 1 },
            new Player { Id = 3, Name = "William Saliba", Position = PlayerPosition.CB, TeamId = 1 },
            new Player { Id = 4, Name = "Gabriel Magalhaes", Position = PlayerPosition.CB, TeamId = 1 },
            new Player { Id = 5, Name = "Oleksandr Zinchenko", Position = PlayerPosition.LB, TeamId = 1 },
            new Player { Id = 6, Name = "Declan Rice", Position = PlayerPosition.CDM, TeamId = 1 },
            new Player { Id = 7, Name = "Thomas Partey", Position = PlayerPosition.CM, TeamId = 1 },
            new Player { Id = 8, Name = "Martin Odegaard", Position = PlayerPosition.CAM, TeamId = 1 },
            new Player { Id = 9, Name = "Bukayo Saka", Position = PlayerPosition.RW, TeamId = 1 },
            new Player { Id = 10, Name = "Gabriel Martinelli", Position = PlayerPosition.LW, TeamId = 1 },
            new Player { Id = 11, Name = "Kai Havertz", Position = PlayerPosition.ST, TeamId = 1 },
            // Manchester City (4-3-3)
            new Player { Id = 12, Name = "Ederson", Position = PlayerPosition.GK, TeamId = 2 },
            new Player { Id = 13, Name = "Kyle Walker", Position = PlayerPosition.RB, TeamId = 2 },
            new Player { Id = 14, Name = "Ruben Dias", Position = PlayerPosition.CB, TeamId = 2 },
            new Player { Id = 15, Name = "John Stones", Position = PlayerPosition.CB, TeamId = 2 },
            new Player { Id = 16, Name = "Josko Gvardiol", Position = PlayerPosition.LB, TeamId = 2 },
            new Player { Id = 17, Name = "Rodri", Position = PlayerPosition.CDM, TeamId = 2 },
            new Player { Id = 18, Name = "Mateo Kovacic", Position = PlayerPosition.CM, TeamId = 2 },
            new Player { Id = 19, Name = "Kevin De Bruyne", Position = PlayerPosition.CAM, TeamId = 2 },
            new Player { Id = 20, Name = "Bernardo Silva", Position = PlayerPosition.RW, TeamId = 2 },
            new Player { Id = 21, Name = "Phil Foden", Position = PlayerPosition.LW, TeamId = 2 },
            new Player { Id = 22, Name = "Erling Haaland", Position = PlayerPosition.ST, TeamId = 2 },
            // Liverpool (4-3-3)
            new Player { Id = 23, Name = "Alisson Becker", Position = PlayerPosition.GK, TeamId = 3 },
            new Player { Id = 24, Name = "Trent Alexander-Arnold", Position = PlayerPosition.RB, TeamId = 3 },
            new Player { Id = 25, Name = "Virgil van Dijk", Position = PlayerPosition.CB, TeamId = 3 },
            new Player { Id = 26, Name = "Ibrahima Konate", Position = PlayerPosition.CB, TeamId = 3 },
            new Player { Id = 27, Name = "Andrew Robertson", Position = PlayerPosition.LB, TeamId = 3 },
            new Player { Id = 28, Name = "Ryan Gravenberch", Position = PlayerPosition.CDM, TeamId = 3 },
            new Player { Id = 29, Name = "Alexis Mac Allister", Position = PlayerPosition.CM, TeamId = 3 },
            new Player { Id = 30, Name = "Dominik Szoboszlai", Position = PlayerPosition.CAM, TeamId = 3 },
            new Player { Id = 31, Name = "Mohamed Salah", Position = PlayerPosition.RW, TeamId = 3 },
            new Player { Id = 32, Name = "Luis Diaz", Position = PlayerPosition.LW, TeamId = 3 },
            new Player { Id = 33, Name = "Darwin Nunez", Position = PlayerPosition.ST, TeamId = 3 },
            // Chelsea (4-3-3)
            new Player { Id = 34, Name = "Robert Sanchez", Position = PlayerPosition.GK, TeamId = 4 },
            new Player { Id = 35, Name = "Reece James", Position = PlayerPosition.RB, TeamId = 4 },
            new Player { Id = 36, Name = "Wesley Fofana", Position = PlayerPosition.CB, TeamId = 4 },
            new Player { Id = 37, Name = "Levi Colwill", Position = PlayerPosition.CB, TeamId = 4 },
            new Player { Id = 38, Name = "Marc Cucurella", Position = PlayerPosition.LB, TeamId = 4 },
            new Player { Id = 39, Name = "Moises Caicedo", Position = PlayerPosition.CDM, TeamId = 4 },
            new Player { Id = 40, Name = "Enzo Fernandez", Position = PlayerPosition.CM, TeamId = 4 },
            new Player { Id = 41, Name = "Christopher Nkunku", Position = PlayerPosition.CAM, TeamId = 4 },
            new Player { Id = 42, Name = "Cole Palmer", Position = PlayerPosition.RW, TeamId = 4 },
            new Player { Id = 43, Name = "Mykhailo Mudryk", Position = PlayerPosition.LW, TeamId = 4 },
            new Player { Id = 44, Name = "Nicolas Jackson", Position = PlayerPosition.ST, TeamId = 4 },
            // Real Madrid (4-3-3)
            new Player { Id = 45, Name = "Thibaut Courtois", Position = PlayerPosition.GK, TeamId = 5 },
            new Player { Id = 46, Name = "Dani Carvajal", Position = PlayerPosition.RB, TeamId = 5 },
            new Player { Id = 47, Name = "Antonio Rudiger", Position = PlayerPosition.CB, TeamId = 5 },
            new Player { Id = 48, Name = "David Alaba", Position = PlayerPosition.CB, TeamId = 5 },
            new Player { Id = 49, Name = "Ferland Mendy", Position = PlayerPosition.LB, TeamId = 5 },
            new Player { Id = 50, Name = "Eduardo Camavinga", Position = PlayerPosition.CDM, TeamId = 5 },
            new Player { Id = 51, Name = "Federico Valverde", Position = PlayerPosition.CM, TeamId = 5 },
            new Player { Id = 52, Name = "Jude Bellingham", Position = PlayerPosition.CAM, TeamId = 5 },
            new Player { Id = 53, Name = "Rodrygo", Position = PlayerPosition.RW, TeamId = 5 },
            new Player { Id = 54, Name = "Vinicius Junior", Position = PlayerPosition.LW, TeamId = 5 },
            new Player { Id = 55, Name = "Kylian Mbappe", Position = PlayerPosition.ST, TeamId = 5 },
            // Barcelona (4-3-3)
            new Player { Id = 56, Name = "Marc-Andre ter Stegen", Position = PlayerPosition.GK, TeamId = 6 },
            new Player { Id = 57, Name = "Jules Kounde", Position = PlayerPosition.RB, TeamId = 6 },
            new Player { Id = 58, Name = "Ronald Araujo", Position = PlayerPosition.CB, TeamId = 6 },
            new Player { Id = 59, Name = "Andreas Christensen", Position = PlayerPosition.CB, TeamId = 6 },
            new Player { Id = 60, Name = "Alejandro Balde", Position = PlayerPosition.LB, TeamId = 6 },
            new Player { Id = 61, Name = "Frenkie de Jong", Position = PlayerPosition.CDM, TeamId = 6 },
            new Player { Id = 62, Name = "Pedri", Position = PlayerPosition.CM, TeamId = 6 },
            new Player { Id = 63, Name = "Gavi", Position = PlayerPosition.CAM, TeamId = 6 },
            new Player { Id = 64, Name = "Raphinha", Position = PlayerPosition.RW, TeamId = 6 },
            new Player { Id = 65, Name = "Lamine Yamal", Position = PlayerPosition.LW, TeamId = 6 },
            new Player { Id = 66, Name = "Robert Lewandowski", Position = PlayerPosition.ST, TeamId = 6 },
            // Atletico Madrid (4-3-3)
            new Player { Id = 67, Name = "Jan Oblak", Position = PlayerPosition.GK, TeamId = 7 },
            new Player { Id = 68, Name = "Nahuel Molina", Position = PlayerPosition.RB, TeamId = 7 },
            new Player { Id = 69, Name = "Jose Gimenez", Position = PlayerPosition.CB, TeamId = 7 },
            new Player { Id = 70, Name = "Axel Witsel", Position = PlayerPosition.CB, TeamId = 7 },
            new Player { Id = 71, Name = "Reinildo Mandava", Position = PlayerPosition.LB, TeamId = 7 },
            new Player { Id = 72, Name = "Rodrigo De Paul", Position = PlayerPosition.CDM, TeamId = 7 },
            new Player { Id = 73, Name = "Koke", Position = PlayerPosition.CM, TeamId = 7 },
            new Player { Id = 74, Name = "Antoine Griezmann", Position = PlayerPosition.CAM, TeamId = 7 },
            new Player { Id = 75, Name = "Angel Correa", Position = PlayerPosition.RW, TeamId = 7 },
            new Player { Id = 76, Name = "Samuel Lino", Position = PlayerPosition.LW, TeamId = 7 },
            new Player { Id = 77, Name = "Julian Alvarez", Position = PlayerPosition.ST, TeamId = 7 },
            // Sevilla (4-3-3)
            new Player { Id = 78, Name = "Orjan Nyland", Position = PlayerPosition.GK, TeamId = 8 },
            new Player { Id = 79, Name = "Jesus Navas", Position = PlayerPosition.RB, TeamId = 8 },
            new Player { Id = 80, Name = "Loic Bade", Position = PlayerPosition.CB, TeamId = 8 },
            new Player { Id = 81, Name = "Tanguy Nianzou", Position = PlayerPosition.CB, TeamId = 8 },
            new Player { Id = 82, Name = "Marcos Acuna", Position = PlayerPosition.LB, TeamId = 8 },
            new Player { Id = 83, Name = "Nemanja Gudelj", Position = PlayerPosition.CDM, TeamId = 8 },
            new Player { Id = 84, Name = "Fernando Reges", Position = PlayerPosition.CM, TeamId = 8 },
            new Player { Id = 85, Name = "Ivan Rakitic", Position = PlayerPosition.CAM, TeamId = 8 },
            new Player { Id = 86, Name = "Dodi Lukebakio", Position = PlayerPosition.RW, TeamId = 8 },
            new Player { Id = 87, Name = "Lucas Ocampos", Position = PlayerPosition.LW, TeamId = 8 },
            new Player { Id = 88, Name = "Youssef En-Nesyri", Position = PlayerPosition.ST, TeamId = 8 });

        var teamIds = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        var matches = new Match[250];

        for (var i = 0; i < matches.Length; i++)
        {
            var homeIndex = i % teamIds.Length;
            var awayIndex = (i + 1 + i / teamIds.Length) % teamIds.Length;
            if (awayIndex == homeIndex)
                awayIndex = (awayIndex + 1) % teamIds.Length;

            matches[i] = new Match
            {
                Id = i + 1,
                HomeTeamId = teamIds[homeIndex],
                AwayTeamId = teamIds[awayIndex]
            };
        }

        modelBuilder.Entity<Match>().HasData(matches);
    }
}