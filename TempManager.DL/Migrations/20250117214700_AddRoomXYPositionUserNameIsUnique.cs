using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TempManager.DL.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomXYPositionUserNameIsUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Uid",
                table: "Users");

            migrationBuilder.AddColumn<double>(
                name: "XPosition",
                table: "Rooms",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "YPosition",
                table: "Rooms",
                type: "double precision",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_UserName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "XPosition",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "YPosition",
                table: "Rooms");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Uid",
                table: "Users",
                column: "Uid",
                unique: true);
        }
    }
}
