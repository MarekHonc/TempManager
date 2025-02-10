using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TempManager.DL.Migrations
{
    /// <inheritdoc />
    public partial class AddViewAndControllAllUserRight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasRightToView",
                table: "UsersToRooms",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanViewAllRooms",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasRightToView",
                table: "UsersToRooms");

            migrationBuilder.DropColumn(
                name: "CanViewAllRooms",
                table: "Users");
        }
    }
}
