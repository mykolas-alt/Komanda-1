using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projektas.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedTimestampAndDifficultiesToGameTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "difficulty",
                table: "sudokuScores",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "mode",
                table: "sudokuScores",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "timestamp",
                table: "sudokuScores",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "pairUpScores",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "difficulty",
                table: "pairUpScores",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "timestamp",
                table: "pairUpScores",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "mathGameScores",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<DateTime>(
                name: "timestamp",
                table: "mathGameScores",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "aimTrainerScores",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "difficulty",
                table: "aimTrainerScores",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "timestamp",
                table: "aimTrainerScores",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "difficulty",
                table: "sudokuScores");

            migrationBuilder.DropColumn(
                name: "mode",
                table: "sudokuScores");

            migrationBuilder.DropColumn(
                name: "timestamp",
                table: "sudokuScores");

            migrationBuilder.DropColumn(
                name: "difficulty",
                table: "pairUpScores");

            migrationBuilder.DropColumn(
                name: "timestamp",
                table: "pairUpScores");

            migrationBuilder.DropColumn(
                name: "timestamp",
                table: "mathGameScores");

            migrationBuilder.DropColumn(
                name: "difficulty",
                table: "aimTrainerScores");

            migrationBuilder.DropColumn(
                name: "timestamp",
                table: "aimTrainerScores");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "pairUpScores",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "mathGameScores",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "aimTrainerScores",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);
        }
    }
}
