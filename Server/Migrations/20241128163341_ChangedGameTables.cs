using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projektas.Server.Migrations
{
    /// <inheritdoc />
    public partial class ChangedGameTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    surname = table.Column<string>(type: "TEXT", nullable: false),
                    username = table.Column<string>(type: "TEXT", nullable: false),
                    password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "aimTrainerScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    score = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aimTrainerScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_aimTrainerScores_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mathGameScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    score = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mathGameScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mathGameScores_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pairUpScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    timeInSeconds = table.Column<int>(type: "INTEGER", nullable: true),
                    fails = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pairUpScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pairUpScores_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sudokuScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    timeInSeconds = table.Column<int>(type: "INTEGER", nullable: true),
                    solved = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sudokuScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sudokuScores_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_aimTrainerScores_UserId",
                table: "aimTrainerScores",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_mathGameScores_UserId",
                table: "mathGameScores",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_pairUpScores_UserId",
                table: "pairUpScores",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_sudokuScores_UserId",
                table: "sudokuScores",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aimTrainerScores");

            migrationBuilder.DropTable(
                name: "mathGameScores");

            migrationBuilder.DropTable(
                name: "pairUpScores");

            migrationBuilder.DropTable(
                name: "sudokuScores");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
