using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTheRoom.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Hotels_HotelId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Rooms_RoomId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_HotelId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RoomId",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Orders_HotelId",
                table: "Orders",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RoomId",
                table: "Orders",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Hotels_HotelId",
                table: "Orders",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Rooms_RoomId",
                table: "Orders",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }
    }
}
