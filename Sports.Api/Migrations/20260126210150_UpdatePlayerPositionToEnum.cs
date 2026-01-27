#nullable disable

namespace Sports.Api.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class UpdatePlayerPositionToEnum : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AlterColumn<int>(
            name: "Position",
            table: "Players",
            type: "int",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(50)");

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.AlterColumn<string>(
            name: "Position",
            table: "Players",
            type: "varchar(50)",
            nullable: false,
            oldClrType: typeof(int));
}
