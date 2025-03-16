using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TempManager.DL.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupsToRoomAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Groups",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Groups",
                table: "Rooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Groups",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Groups",
                table: "Rooms");
        }
    }
}
