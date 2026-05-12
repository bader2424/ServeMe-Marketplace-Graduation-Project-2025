using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SERVE_ME_PROJECT.Migrations
{
    /// <inheritdoc />
    public partial class ServMeDb1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TbRole_RoleModelId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "TbRole");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RoleModelId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RoleModelId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "TransactionTypes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsCredit",
                table: "TransactionTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "TransactionTypes");

            migrationBuilder.DropColumn(
                name: "IsCredit",
                table: "TransactionTypes");

            migrationBuilder.AddColumn<int>(
                name: "RoleModelId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TbRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbRole", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RoleModelId",
                table: "AspNetUsers",
                column: "RoleModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TbRole_RoleModelId",
                table: "AspNetUsers",
                column: "RoleModelId",
                principalTable: "TbRole",
                principalColumn: "Id");
        }
    }
}
