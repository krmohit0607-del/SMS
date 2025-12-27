using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMS.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrachers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Teachers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_UserId1",
                table: "Teachers",
                column: "UserId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Users_UserId1",
                table: "Teachers",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Users_UserId1",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_UserId1",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Teachers");
        }
    }
}
