using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERVE_ME_PROJECT.Migrations
{
    /// <inheritdoc />
    public partial class tajerr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RentalDays",
                table: "TbOrder",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RentalStartDate",
                table: "TbOrder",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentalDays",
                table: "TbOrder");

            migrationBuilder.DropColumn(
                name: "RentalStartDate",
                table: "TbOrder");
        }
    }
}
