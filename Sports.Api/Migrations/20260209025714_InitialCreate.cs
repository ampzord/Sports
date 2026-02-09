using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Sports.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LeagueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HomeTeamId = table.Column<int>(type: "int", nullable: false),
                    AwayTeamId = table.Column<int>(type: "int", nullable: false),
                    TotalPasses = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_AwayTeamId",
                        column: x => x.AwayTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_HomeTeamId",
                        column: x => x.HomeTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Leagues",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Premier League" },
                    { 2, "La Liga" }
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "LeagueId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Arsenal" },
                    { 2, 1, "Manchester City" },
                    { 3, 1, "Liverpool" },
                    { 4, 1, "Chelsea" },
                    { 5, 2, "Real Madrid" },
                    { 6, 2, "Barcelona" },
                    { 7, 2, "Atletico Madrid" },
                    { 8, 2, "Sevilla" }
                });

            migrationBuilder.InsertData(
                table: "Matches",
                columns: new[] { "Id", "AwayTeamId", "HomeTeamId", "TotalPasses" },
                values: new object[,]
                {
                    { 1, 2, 1, null },
                    { 2, 3, 2, null },
                    { 3, 4, 3, null },
                    { 4, 5, 4, null },
                    { 5, 6, 5, null },
                    { 6, 7, 6, null },
                    { 7, 8, 7, null },
                    { 8, 1, 8, null },
                    { 9, 3, 1, null },
                    { 10, 4, 2, null },
                    { 11, 5, 3, null },
                    { 12, 6, 4, null },
                    { 13, 7, 5, null },
                    { 14, 8, 6, null },
                    { 15, 1, 7, null },
                    { 16, 2, 8, null },
                    { 17, 4, 1, null },
                    { 18, 5, 2, null },
                    { 19, 6, 3, null },
                    { 20, 7, 4, null },
                    { 21, 8, 5, null },
                    { 22, 1, 6, null },
                    { 23, 2, 7, null },
                    { 24, 3, 8, null },
                    { 25, 5, 1, null },
                    { 26, 6, 2, null },
                    { 27, 7, 3, null },
                    { 28, 8, 4, null },
                    { 29, 1, 5, null },
                    { 30, 2, 6, null },
                    { 31, 3, 7, null },
                    { 32, 4, 8, null },
                    { 33, 6, 1, null },
                    { 34, 7, 2, null },
                    { 35, 8, 3, null },
                    { 36, 1, 4, null },
                    { 37, 2, 5, null },
                    { 38, 3, 6, null },
                    { 39, 4, 7, null },
                    { 40, 5, 8, null },
                    { 41, 7, 1, null },
                    { 42, 8, 2, null },
                    { 43, 1, 3, null },
                    { 44, 2, 4, null },
                    { 45, 3, 5, null },
                    { 46, 4, 6, null },
                    { 47, 5, 7, null },
                    { 48, 6, 8, null },
                    { 49, 8, 1, null },
                    { 50, 1, 2, null },
                    { 51, 2, 3, null },
                    { 52, 3, 4, null },
                    { 53, 4, 5, null },
                    { 54, 5, 6, null },
                    { 55, 6, 7, null },
                    { 56, 7, 8, null },
                    { 57, 2, 1, null },
                    { 58, 3, 2, null },
                    { 59, 4, 3, null },
                    { 60, 5, 4, null },
                    { 61, 6, 5, null },
                    { 62, 7, 6, null },
                    { 63, 8, 7, null },
                    { 64, 1, 8, null },
                    { 65, 2, 1, null },
                    { 66, 3, 2, null },
                    { 67, 4, 3, null },
                    { 68, 5, 4, null },
                    { 69, 6, 5, null },
                    { 70, 7, 6, null },
                    { 71, 8, 7, null },
                    { 72, 1, 8, null },
                    { 73, 3, 1, null },
                    { 74, 4, 2, null },
                    { 75, 5, 3, null },
                    { 76, 6, 4, null },
                    { 77, 7, 5, null },
                    { 78, 8, 6, null },
                    { 79, 1, 7, null },
                    { 80, 2, 8, null },
                    { 81, 4, 1, null },
                    { 82, 5, 2, null },
                    { 83, 6, 3, null },
                    { 84, 7, 4, null },
                    { 85, 8, 5, null },
                    { 86, 1, 6, null },
                    { 87, 2, 7, null },
                    { 88, 3, 8, null },
                    { 89, 5, 1, null },
                    { 90, 6, 2, null },
                    { 91, 7, 3, null },
                    { 92, 8, 4, null },
                    { 93, 1, 5, null },
                    { 94, 2, 6, null },
                    { 95, 3, 7, null },
                    { 96, 4, 8, null },
                    { 97, 6, 1, null },
                    { 98, 7, 2, null },
                    { 99, 8, 3, null },
                    { 100, 1, 4, null },
                    { 101, 2, 5, null },
                    { 102, 3, 6, null },
                    { 103, 4, 7, null },
                    { 104, 5, 8, null },
                    { 105, 7, 1, null },
                    { 106, 8, 2, null },
                    { 107, 1, 3, null },
                    { 108, 2, 4, null },
                    { 109, 3, 5, null },
                    { 110, 4, 6, null },
                    { 111, 5, 7, null },
                    { 112, 6, 8, null },
                    { 113, 8, 1, null },
                    { 114, 1, 2, null },
                    { 115, 2, 3, null },
                    { 116, 3, 4, null },
                    { 117, 4, 5, null },
                    { 118, 5, 6, null },
                    { 119, 6, 7, null },
                    { 120, 7, 8, null },
                    { 121, 2, 1, null },
                    { 122, 3, 2, null },
                    { 123, 4, 3, null },
                    { 124, 5, 4, null },
                    { 125, 6, 5, null },
                    { 126, 7, 6, null },
                    { 127, 8, 7, null },
                    { 128, 1, 8, null },
                    { 129, 2, 1, null },
                    { 130, 3, 2, null },
                    { 131, 4, 3, null },
                    { 132, 5, 4, null },
                    { 133, 6, 5, null },
                    { 134, 7, 6, null },
                    { 135, 8, 7, null },
                    { 136, 1, 8, null },
                    { 137, 3, 1, null },
                    { 138, 4, 2, null },
                    { 139, 5, 3, null },
                    { 140, 6, 4, null },
                    { 141, 7, 5, null },
                    { 142, 8, 6, null },
                    { 143, 1, 7, null },
                    { 144, 2, 8, null },
                    { 145, 4, 1, null },
                    { 146, 5, 2, null },
                    { 147, 6, 3, null },
                    { 148, 7, 4, null },
                    { 149, 8, 5, null },
                    { 150, 1, 6, null },
                    { 151, 2, 7, null },
                    { 152, 3, 8, null },
                    { 153, 5, 1, null },
                    { 154, 6, 2, null },
                    { 155, 7, 3, null },
                    { 156, 8, 4, null },
                    { 157, 1, 5, null },
                    { 158, 2, 6, null },
                    { 159, 3, 7, null },
                    { 160, 4, 8, null },
                    { 161, 6, 1, null },
                    { 162, 7, 2, null },
                    { 163, 8, 3, null },
                    { 164, 1, 4, null },
                    { 165, 2, 5, null },
                    { 166, 3, 6, null },
                    { 167, 4, 7, null },
                    { 168, 5, 8, null },
                    { 169, 7, 1, null },
                    { 170, 8, 2, null },
                    { 171, 1, 3, null },
                    { 172, 2, 4, null },
                    { 173, 3, 5, null },
                    { 174, 4, 6, null },
                    { 175, 5, 7, null },
                    { 176, 6, 8, null },
                    { 177, 8, 1, null },
                    { 178, 1, 2, null },
                    { 179, 2, 3, null },
                    { 180, 3, 4, null },
                    { 181, 4, 5, null },
                    { 182, 5, 6, null },
                    { 183, 6, 7, null },
                    { 184, 7, 8, null },
                    { 185, 2, 1, null },
                    { 186, 3, 2, null },
                    { 187, 4, 3, null },
                    { 188, 5, 4, null },
                    { 189, 6, 5, null },
                    { 190, 7, 6, null },
                    { 191, 8, 7, null },
                    { 192, 1, 8, null },
                    { 193, 2, 1, null },
                    { 194, 3, 2, null },
                    { 195, 4, 3, null },
                    { 196, 5, 4, null },
                    { 197, 6, 5, null },
                    { 198, 7, 6, null },
                    { 199, 8, 7, null },
                    { 200, 1, 8, null },
                    { 201, 3, 1, null },
                    { 202, 4, 2, null },
                    { 203, 5, 3, null },
                    { 204, 6, 4, null },
                    { 205, 7, 5, null },
                    { 206, 8, 6, null },
                    { 207, 1, 7, null },
                    { 208, 2, 8, null },
                    { 209, 4, 1, null },
                    { 210, 5, 2, null },
                    { 211, 6, 3, null },
                    { 212, 7, 4, null },
                    { 213, 8, 5, null },
                    { 214, 1, 6, null },
                    { 215, 2, 7, null },
                    { 216, 3, 8, null },
                    { 217, 5, 1, null },
                    { 218, 6, 2, null },
                    { 219, 7, 3, null },
                    { 220, 8, 4, null },
                    { 221, 1, 5, null },
                    { 222, 2, 6, null },
                    { 223, 3, 7, null },
                    { 224, 4, 8, null },
                    { 225, 6, 1, null },
                    { 226, 7, 2, null },
                    { 227, 8, 3, null },
                    { 228, 1, 4, null },
                    { 229, 2, 5, null },
                    { 230, 3, 6, null },
                    { 231, 4, 7, null },
                    { 232, 5, 8, null },
                    { 233, 7, 1, null },
                    { 234, 8, 2, null },
                    { 235, 1, 3, null },
                    { 236, 2, 4, null },
                    { 237, 3, 5, null },
                    { 238, 4, 6, null },
                    { 239, 5, 7, null },
                    { 240, 6, 8, null },
                    { 241, 8, 1, null },
                    { 242, 1, 2, null },
                    { 243, 2, 3, null },
                    { 244, 3, 4, null },
                    { 245, 4, 5, null },
                    { 246, 5, 6, null },
                    { 247, 6, 7, null },
                    { 248, 7, 8, null },
                    { 249, 2, 1, null },
                    { 250, 3, 2, null }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Name", "Position", "TeamId" },
                values: new object[,]
                {
                    { 1, "David Raya", "GK", 1 },
                    { 2, "Ben White", "RB", 1 },
                    { 3, "William Saliba", "CB", 1 },
                    { 4, "Gabriel Magalhaes", "CB", 1 },
                    { 5, "Oleksandr Zinchenko", "LB", 1 },
                    { 6, "Declan Rice", "CDM", 1 },
                    { 7, "Thomas Partey", "CM", 1 },
                    { 8, "Martin Odegaard", "CAM", 1 },
                    { 9, "Bukayo Saka", "RW", 1 },
                    { 10, "Gabriel Martinelli", "LW", 1 },
                    { 11, "Kai Havertz", "ST", 1 },
                    { 12, "Ederson", "GK", 2 },
                    { 13, "Kyle Walker", "RB", 2 },
                    { 14, "Ruben Dias", "CB", 2 },
                    { 15, "John Stones", "CB", 2 },
                    { 16, "Josko Gvardiol", "LB", 2 },
                    { 17, "Rodri", "CDM", 2 },
                    { 18, "Mateo Kovacic", "CM", 2 },
                    { 19, "Kevin De Bruyne", "CAM", 2 },
                    { 20, "Bernardo Silva", "RW", 2 },
                    { 21, "Phil Foden", "LW", 2 },
                    { 22, "Erling Haaland", "ST", 2 },
                    { 23, "Alisson Becker", "GK", 3 },
                    { 24, "Trent Alexander-Arnold", "RB", 3 },
                    { 25, "Virgil van Dijk", "CB", 3 },
                    { 26, "Ibrahima Konate", "CB", 3 },
                    { 27, "Andrew Robertson", "LB", 3 },
                    { 28, "Ryan Gravenberch", "CDM", 3 },
                    { 29, "Alexis Mac Allister", "CM", 3 },
                    { 30, "Dominik Szoboszlai", "CAM", 3 },
                    { 31, "Mohamed Salah", "RW", 3 },
                    { 32, "Luis Diaz", "LW", 3 },
                    { 33, "Darwin Nunez", "ST", 3 },
                    { 34, "Robert Sanchez", "GK", 4 },
                    { 35, "Reece James", "RB", 4 },
                    { 36, "Wesley Fofana", "CB", 4 },
                    { 37, "Levi Colwill", "CB", 4 },
                    { 38, "Marc Cucurella", "LB", 4 },
                    { 39, "Moises Caicedo", "CDM", 4 },
                    { 40, "Enzo Fernandez", "CM", 4 },
                    { 41, "Christopher Nkunku", "CAM", 4 },
                    { 42, "Cole Palmer", "RW", 4 },
                    { 43, "Mykhailo Mudryk", "LW", 4 },
                    { 44, "Nicolas Jackson", "ST", 4 },
                    { 45, "Thibaut Courtois", "GK", 5 },
                    { 46, "Dani Carvajal", "RB", 5 },
                    { 47, "Antonio Rudiger", "CB", 5 },
                    { 48, "David Alaba", "CB", 5 },
                    { 49, "Ferland Mendy", "LB", 5 },
                    { 50, "Eduardo Camavinga", "CDM", 5 },
                    { 51, "Federico Valverde", "CM", 5 },
                    { 52, "Jude Bellingham", "CAM", 5 },
                    { 53, "Rodrygo", "RW", 5 },
                    { 54, "Vinicius Junior", "LW", 5 },
                    { 55, "Kylian Mbappe", "ST", 5 },
                    { 56, "Marc-Andre ter Stegen", "GK", 6 },
                    { 57, "Jules Kounde", "RB", 6 },
                    { 58, "Ronald Araujo", "CB", 6 },
                    { 59, "Andreas Christensen", "CB", 6 },
                    { 60, "Alejandro Balde", "LB", 6 },
                    { 61, "Frenkie de Jong", "CDM", 6 },
                    { 62, "Pedri", "CM", 6 },
                    { 63, "Gavi", "CAM", 6 },
                    { 64, "Raphinha", "RW", 6 },
                    { 65, "Lamine Yamal", "LW", 6 },
                    { 66, "Robert Lewandowski", "ST", 6 },
                    { 67, "Jan Oblak", "GK", 7 },
                    { 68, "Nahuel Molina", "RB", 7 },
                    { 69, "Jose Gimenez", "CB", 7 },
                    { 70, "Axel Witsel", "CB", 7 },
                    { 71, "Reinildo Mandava", "LB", 7 },
                    { 72, "Rodrigo De Paul", "CDM", 7 },
                    { 73, "Koke", "CM", 7 },
                    { 74, "Antoine Griezmann", "CAM", 7 },
                    { 75, "Angel Correa", "RW", 7 },
                    { 76, "Samuel Lino", "LW", 7 },
                    { 77, "Julian Alvarez", "ST", 7 },
                    { 78, "Orjan Nyland", "GK", 8 },
                    { 79, "Jesus Navas", "RB", 8 },
                    { 80, "Loic Bade", "CB", 8 },
                    { 81, "Tanguy Nianzou", "CB", 8 },
                    { 82, "Marcos Acuna", "LB", 8 },
                    { 83, "Nemanja Gudelj", "CDM", 8 },
                    { 84, "Fernando Reges", "CM", 8 },
                    { 85, "Ivan Rakitic", "CAM", 8 },
                    { 86, "Dodi Lukebakio", "RW", 8 },
                    { 87, "Lucas Ocampos", "LW", 8 },
                    { 88, "Youssef En-Nesyri", "ST", 8 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_Name",
                table: "Leagues",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_AwayTeamId",
                table: "Matches",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomeTeamId",
                table: "Matches",
                column: "HomeTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_Name",
                table: "Players",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId",
                table: "Players",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_LeagueId",
                table: "Teams",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_Name",
                table: "Teams",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Leagues");
        }
    }
}
