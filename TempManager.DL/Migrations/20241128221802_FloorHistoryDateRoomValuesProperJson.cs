using Microsoft.EntityFrameworkCore.Migrations;
using TempManager.DL.Entities.JsonTypes;

#nullable disable

namespace TempManager.DL.Migrations
{
    /// <inheritdoc />
    public partial class FloorHistoryDateRoomValuesProperJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RoomValues",
                table: "FloorHistories",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(RoomValue[]),
                oldType: "jsonb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<RoomValue[]>(
                name: "RoomValues",
                table: "FloorHistories",
                type: "jsonb",
                nullable: false,
                defaultValue: new RoomValue[0],
                oldClrType: typeof(string),
                oldType: "jsonb",
                oldNullable: true);
        }
    }
}
