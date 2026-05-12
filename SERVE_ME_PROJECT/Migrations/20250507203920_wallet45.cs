using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERVE_ME_PROJECT.Migrations
{
    /// <inheritdoc />
    public partial class wallet45 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminNotes",
                table: "WithdrawalRequests");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "WalletTransfers");

            migrationBuilder.DropColumn(
                name: "AdminNotes",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "AdminNotes",
                table: "RefundRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminNotes",
                table: "WithdrawalRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "WalletTransfers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminNotes",
                table: "WalletTransactions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminNotes",
                table: "RefundRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
