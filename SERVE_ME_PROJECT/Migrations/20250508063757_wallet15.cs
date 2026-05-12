using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERVE_ME_PROJECT.Migrations
{
    /// <inheritdoc />
    public partial class wallet15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbOrder_TbOrderStatModel_OrderStatId",
                table: "TbOrder");

            migrationBuilder.DropIndex(
                name: "IX_TbOrder_OrderStatId",
                table: "TbOrder");

            migrationBuilder.DropColumn(
                name: "OrderStatId",
                table: "TbOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TbOrder_StatOrderId",
                table: "TbOrder",
                column: "StatOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrder_TbOrderStatModel_StatOrderId",
                table: "TbOrder",
                column: "StatOrderId",
                principalTable: "TbOrderStatModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbOrder_TbOrderStatModel_StatOrderId",
                table: "TbOrder");

            migrationBuilder.DropIndex(
                name: "IX_TbOrder_StatOrderId",
                table: "TbOrder");

            migrationBuilder.AddColumn<int>(
                name: "OrderStatId",
                table: "TbOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TbOrder_OrderStatId",
                table: "TbOrder",
                column: "OrderStatId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbOrder_TbOrderStatModel_OrderStatId",
                table: "TbOrder",
                column: "OrderStatId",
                principalTable: "TbOrderStatModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
