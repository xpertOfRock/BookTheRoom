using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Upd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP WITH TIME ZONE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "TIMESTAMP WITH TIME ZONE",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
