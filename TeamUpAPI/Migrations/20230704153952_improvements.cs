using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamUpAPI.Migrations
{
    /// <inheritdoc />
    public partial class improvements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_PlayTimesAvailability_Users_UserId",
                table: "PlayTimesAvailability",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayTimesAvailability_Users_UserId",
                table: "PlayTimesAvailability");
        }
    }
}
