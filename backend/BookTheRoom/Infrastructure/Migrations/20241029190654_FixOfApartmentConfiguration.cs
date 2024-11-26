using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixOfApartmentConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Apartments_HotelId",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ApartmentId",
                table: "Comments",
                column: "ApartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Apartments_ApartmentId",
                table: "Comments",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Apartments_ApartmentId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ApartmentId",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Apartments_HotelId",
                table: "Comments",
                column: "HotelId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
