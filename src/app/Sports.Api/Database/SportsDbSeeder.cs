namespace Sports.Api.Database;

using Microsoft.EntityFrameworkCore;
using Sports.Domain.LeagueAggregate;
using Sports.Domain.MatchAggregate;
using Sports.Domain.PlayerAggregate;
using Sports.Domain.PlayerAggregate.Enums;
using Sports.Domain.TeamAggregate;
using Sports.Domain.TeamAggregate.ValueObjects;

public static class SportsDbSeeder
{
    public static async Task SeedAsync(SportsDbContext db)
    {
        if (await db.Leagues.AnyAsync())
            return;

        var premierLeague = League.Create("Premier League");
        var laLiga = League.Create("La Liga");
        db.Leagues.AddRange(premierLeague, laLiga);

        var arsenal = Team.Create("Arsenal", premierLeague.Id);
        var manchesterCity = Team.Create("Manchester City", premierLeague.Id);
        var liverpool = Team.Create("Liverpool", premierLeague.Id);
        var chelsea = Team.Create("Chelsea", premierLeague.Id);
        var realMadrid = Team.Create("Real Madrid", laLiga.Id);
        var barcelona = Team.Create("Barcelona", laLiga.Id);
        var atleticoMadrid = Team.Create("Atletico Madrid", laLiga.Id);
        var sevilla = Team.Create("Sevilla", laLiga.Id);
        db.Teams.AddRange(arsenal, manchesterCity, liverpool, chelsea,
            realMadrid, barcelona, atleticoMadrid, sevilla);

        SeedPlayers(db, arsenal.Id, manchesterCity.Id, liverpool.Id, chelsea.Id,
            realMadrid.Id, barcelona.Id, atleticoMadrid.Id, sevilla.Id);

        var teams = new[] { arsenal, manchesterCity, liverpool, chelsea,
            realMadrid, barcelona, atleticoMadrid, sevilla };
        SeedMatches(db, teams);

        await db.SaveChangesAsync();
    }

    private static void SeedPlayers(SportsDbContext db,
        TeamId arsenal, TeamId manchesterCity, TeamId liverpool, TeamId chelsea,
        TeamId realMadrid, TeamId barcelona, TeamId atleticoMadrid, TeamId sevilla) => db.Players.AddRange(
            // Arsenal (4-3-3)
            Player.Create("David Raya", PlayerPosition.GK, arsenal),
            Player.Create("Ben White", PlayerPosition.RB, arsenal),
            Player.Create("William Saliba", PlayerPosition.CB, arsenal),
            Player.Create("Gabriel Magalhaes", PlayerPosition.CB, arsenal),
            Player.Create("Oleksandr Zinchenko", PlayerPosition.LB, arsenal),
            Player.Create("Declan Rice", PlayerPosition.CDM, arsenal),
            Player.Create("Thomas Partey", PlayerPosition.CM, arsenal),
            Player.Create("Martin Odegaard", PlayerPosition.CAM, arsenal),
            Player.Create("Bukayo Saka", PlayerPosition.RW, arsenal),
            Player.Create("Gabriel Martinelli", PlayerPosition.LW, arsenal),
            Player.Create("Kai Havertz", PlayerPosition.ST, arsenal),
            // Manchester City (4-3-3)
            Player.Create("Ederson", PlayerPosition.GK, manchesterCity),
            Player.Create("Kyle Walker", PlayerPosition.RB, manchesterCity),
            Player.Create("Ruben Dias", PlayerPosition.CB, manchesterCity),
            Player.Create("John Stones", PlayerPosition.CB, manchesterCity),
            Player.Create("Josko Gvardiol", PlayerPosition.LB, manchesterCity),
            Player.Create("Rodri", PlayerPosition.CDM, manchesterCity),
            Player.Create("Mateo Kovacic", PlayerPosition.CM, manchesterCity),
            Player.Create("Kevin De Bruyne", PlayerPosition.CAM, manchesterCity),
            Player.Create("Bernardo Silva", PlayerPosition.RW, manchesterCity),
            Player.Create("Phil Foden", PlayerPosition.LW, manchesterCity),
            Player.Create("Erling Haaland", PlayerPosition.ST, manchesterCity),
            // Liverpool (4-3-3)
            Player.Create("Alisson Becker", PlayerPosition.GK, liverpool),
            Player.Create("Trent Alexander-Arnold", PlayerPosition.RB, liverpool),
            Player.Create("Virgil van Dijk", PlayerPosition.CB, liverpool),
            Player.Create("Ibrahima Konate", PlayerPosition.CB, liverpool),
            Player.Create("Andrew Robertson", PlayerPosition.LB, liverpool),
            Player.Create("Ryan Gravenberch", PlayerPosition.CDM, liverpool),
            Player.Create("Alexis Mac Allister", PlayerPosition.CM, liverpool),
            Player.Create("Dominik Szoboszlai", PlayerPosition.CAM, liverpool),
            Player.Create("Mohamed Salah", PlayerPosition.RW, liverpool),
            Player.Create("Luis Diaz", PlayerPosition.LW, liverpool),
            Player.Create("Darwin Nunez", PlayerPosition.ST, liverpool),
            // Chelsea (4-3-3)
            Player.Create("Robert Sanchez", PlayerPosition.GK, chelsea),
            Player.Create("Reece James", PlayerPosition.RB, chelsea),
            Player.Create("Wesley Fofana", PlayerPosition.CB, chelsea),
            Player.Create("Levi Colwill", PlayerPosition.CB, chelsea),
            Player.Create("Marc Cucurella", PlayerPosition.LB, chelsea),
            Player.Create("Moises Caicedo", PlayerPosition.CDM, chelsea),
            Player.Create("Enzo Fernandez", PlayerPosition.CM, chelsea),
            Player.Create("Christopher Nkunku", PlayerPosition.CAM, chelsea),
            Player.Create("Cole Palmer", PlayerPosition.RW, chelsea),
            Player.Create("Mykhailo Mudryk", PlayerPosition.LW, chelsea),
            Player.Create("Nicolas Jackson", PlayerPosition.ST, chelsea),
            // Real Madrid (4-3-3)
            Player.Create("Thibaut Courtois", PlayerPosition.GK, realMadrid),
            Player.Create("Dani Carvajal", PlayerPosition.RB, realMadrid),
            Player.Create("Antonio Rudiger", PlayerPosition.CB, realMadrid),
            Player.Create("David Alaba", PlayerPosition.CB, realMadrid),
            Player.Create("Ferland Mendy", PlayerPosition.LB, realMadrid),
            Player.Create("Eduardo Camavinga", PlayerPosition.CDM, realMadrid),
            Player.Create("Federico Valverde", PlayerPosition.CM, realMadrid),
            Player.Create("Jude Bellingham", PlayerPosition.CAM, realMadrid),
            Player.Create("Rodrygo", PlayerPosition.RW, realMadrid),
            Player.Create("Vinicius Junior", PlayerPosition.LW, realMadrid),
            Player.Create("Kylian Mbappe", PlayerPosition.ST, realMadrid),
            // Barcelona (4-3-3)
            Player.Create("Marc-Andre ter Stegen", PlayerPosition.GK, barcelona),
            Player.Create("Jules Kounde", PlayerPosition.RB, barcelona),
            Player.Create("Ronald Araujo", PlayerPosition.CB, barcelona),
            Player.Create("Andreas Christensen", PlayerPosition.CB, barcelona),
            Player.Create("Alejandro Balde", PlayerPosition.LB, barcelona),
            Player.Create("Frenkie de Jong", PlayerPosition.CDM, barcelona),
            Player.Create("Pedri", PlayerPosition.CM, barcelona),
            Player.Create("Gavi", PlayerPosition.CAM, barcelona),
            Player.Create("Raphinha", PlayerPosition.RW, barcelona),
            Player.Create("Lamine Yamal", PlayerPosition.LW, barcelona),
            Player.Create("Robert Lewandowski", PlayerPosition.ST, barcelona),
            // Atletico Madrid (4-3-3)
            Player.Create("Jan Oblak", PlayerPosition.GK, atleticoMadrid),
            Player.Create("Nahuel Molina", PlayerPosition.RB, atleticoMadrid),
            Player.Create("Jose Gimenez", PlayerPosition.CB, atleticoMadrid),
            Player.Create("Axel Witsel", PlayerPosition.CB, atleticoMadrid),
            Player.Create("Reinildo Mandava", PlayerPosition.LB, atleticoMadrid),
            Player.Create("Rodrigo De Paul", PlayerPosition.CDM, atleticoMadrid),
            Player.Create("Koke", PlayerPosition.CM, atleticoMadrid),
            Player.Create("Antoine Griezmann", PlayerPosition.CAM, atleticoMadrid),
            Player.Create("Angel Correa", PlayerPosition.RW, atleticoMadrid),
            Player.Create("Samuel Lino", PlayerPosition.LW, atleticoMadrid),
            Player.Create("Julian Alvarez", PlayerPosition.ST, atleticoMadrid),
            // Sevilla (4-3-3)
            Player.Create("Orjan Nyland", PlayerPosition.GK, sevilla),
            Player.Create("Jesus Navas", PlayerPosition.RB, sevilla),
            Player.Create("Loic Bade", PlayerPosition.CB, sevilla),
            Player.Create("Tanguy Nianzou", PlayerPosition.CB, sevilla),
            Player.Create("Marcos Acuna", PlayerPosition.LB, sevilla),
            Player.Create("Nemanja Gudelj", PlayerPosition.CDM, sevilla),
            Player.Create("Fernando Reges", PlayerPosition.CM, sevilla),
            Player.Create("Ivan Rakitic", PlayerPosition.CAM, sevilla),
            Player.Create("Dodi Lukebakio", PlayerPosition.RW, sevilla),
            Player.Create("Lucas Ocampos", PlayerPosition.LW, sevilla),
            Player.Create("Youssef En-Nesyri", PlayerPosition.ST, sevilla));

    private static void SeedMatches(SportsDbContext db, Team[] teams)
    {
        for (var i = 0; i < 250; i++)
        {
            var homeIndex = i % teams.Length;
            var awayIndex = (i + 1 + (i / teams.Length)) % teams.Length;
            if (awayIndex == homeIndex)
                awayIndex = (awayIndex + 1) % teams.Length;

            db.Matches.Add(Match.Create(teams[homeIndex].Id, teams[awayIndex].Id));
        }
    }
}
