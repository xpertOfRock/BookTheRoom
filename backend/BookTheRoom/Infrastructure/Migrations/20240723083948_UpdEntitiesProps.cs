using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdEntitiesProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AnonymousNumber",
                table: "Orders",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "AnonymousEmail",
                table: "Orders",
                newName: "Email");

            migrationBuilder.AddColumn<bool>(
                name: "MealsIncluded",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MinibarIncluded",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MealsIncluded",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MinibarIncluded",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Orders",
                newName: "AnonymousNumber");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Orders",
                newName: "AnonymousEmail");
        }
    }
}
