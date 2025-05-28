using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCommentEntityDeleteHotelAndApartmentProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Apartments_ApartmentId1",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Hotels_HotelId1",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ApartmentId1",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_HotelId1",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ApartmentId1",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "HotelId1",
                table: "Comments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApartmentId1",
                table: "Comments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HotelId1",
                table: "Comments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ApartmentId1",
                table: "Comments",
                column: "ApartmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_HotelId1",
                table: "Comments",
                column: "HotelId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Apartments_ApartmentId1",
                table: "Comments",
                column: "ApartmentId1",
                principalTable: "Apartments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Hotels_HotelId1",
                table: "Comments",
                column: "HotelId1",
                principalTable: "Hotels",
                principalColumn: "Id");
        }
    }
}
