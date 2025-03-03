using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DungeonDates.Function.Infrastructure.Databases.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DungeonDates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DungeonDates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProposedDates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DungeonDateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposedDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProposedDates_DungeonDates_DungeonDateId",
                        column: x => x.DungeonDateId,
                        principalTable: "DungeonDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProposedDateResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Accepted = table.Column<bool>(type: "bit", nullable: false),
                    ProposedDateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposedDateResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProposedDateResponses_ProposedDates_ProposedDateId",
                        column: x => x.ProposedDateId,
                        principalTable: "ProposedDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProposedDateResponses_ProposedDateId",
                table: "ProposedDateResponses",
                column: "ProposedDateId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposedDates_DungeonDateId",
                table: "ProposedDates",
                column: "DungeonDateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProposedDateResponses");

            migrationBuilder.DropTable(
                name: "ProposedDates");

            migrationBuilder.DropTable(
                name: "DungeonDates");
        }
    }
}
