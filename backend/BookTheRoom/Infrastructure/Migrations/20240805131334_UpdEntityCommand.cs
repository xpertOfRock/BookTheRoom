using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdEntityCommand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Hotels_HotelId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Hotels_HotelId1",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_HotelId1",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "HotelId1",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Hotels_HotelId",
                table: "Comments",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Hotels_HotelId",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "HotelId1",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_HotelId1",
                table: "Comments",
                column: "HotelId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Hotels_HotelId",
                table: "Comments",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Hotels_HotelId1",
                table: "Comments",
                column: "HotelId1",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
