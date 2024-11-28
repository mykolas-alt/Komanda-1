using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projektas.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddDifficultyToScores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Difficulty",
                table: "sudokuScores",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "sudokuScores",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Difficulty",
                table: "pairUpScores",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "pairUpScores",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Difficulty",
                table: "mathGameScores",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "mathGameScores",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Difficulty",
                table: "aimTrainerScores",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "aimTrainerScores",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "sudokuScores");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "sudokuScores");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "pairUpScores");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "pairUpScores");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "mathGameScores");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "mathGameScores");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "aimTrainerScores");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "aimTrainerScores");
        }
    }
}
