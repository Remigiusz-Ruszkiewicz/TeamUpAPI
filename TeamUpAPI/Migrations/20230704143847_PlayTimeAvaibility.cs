using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamUpAPI.Migrations
{
    /// <inheritdoc />
    public partial class PlayTimeAvaibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayTimesAvailability",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StartHour = table.Column<int>(type: "INTEGER", nullable: false),
                    EndHour = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayTimesAvailability", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayTimesAvailability");
        }
    }
}
