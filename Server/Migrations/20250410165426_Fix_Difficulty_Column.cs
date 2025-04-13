using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projektas.Server.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Difficulty_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GameData_Difficulty",
                table: "mathGameScores",
                newName: "difficulty");

            migrationBuilder.AlterColumn<string>(
                name: "difficulty",
                table: "mathGameScores",
                type: "TEXT",
                nullable: true,
                defaultValue: "Easy",
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "difficulty",
                table: "mathGameScores",
                newName: "GameData_Difficulty");

            migrationBuilder.AlterColumn<int>(
                name: "GameData_Difficulty",
                table: "mathGameScores",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: "Easy");
        }
    }
}
