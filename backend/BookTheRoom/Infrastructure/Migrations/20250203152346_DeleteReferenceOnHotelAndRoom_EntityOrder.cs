using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteReferenceOnHotelAndRoom_EntityOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Hotels_HotelId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Rooms_HotelId_RoomNumber",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_HotelId_RoomNumber",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Orders_HotelId_RoomNumber",
                table: "Orders",
                columns: new[] { "HotelId", "RoomNumber" });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Hotels_HotelId",
                table: "Orders",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Rooms_HotelId_RoomNumber",
                table: "Orders",
                columns: new[] { "HotelId", "RoomNumber" },
                principalTable: "Rooms",
                principalColumns: new[] { "HotelId", "Number" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
