using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beacon.Migrations
{
    /// <inheritdoc />
    public partial class fix_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dev_updates_AspNetUsers_admin_id",
                table: "dev_updates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dev_updates",
                table: "dev_updates");

            migrationBuilder.RenameTable(
                name: "dev_updates",
                newName: "DevUpdate");

            migrationBuilder.RenameIndex(
                name: "IX_dev_updates_admin_id",
                table: "DevUpdate",
                newName: "IX_DevUpdate_admin_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DevUpdate",
                table: "DevUpdate",
                column: "dev_update_id");

            migrationBuilder.AddForeignKey(
                name: "FK_DevUpdate_AspNetUsers_admin_id",
                table: "DevUpdate",
                column: "admin_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DevUpdate_AspNetUsers_admin_id",
                table: "DevUpdate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DevUpdate",
                table: "DevUpdate");

            migrationBuilder.RenameTable(
                name: "DevUpdate",
                newName: "dev_updates");

            migrationBuilder.RenameIndex(
                name: "IX_DevUpdate_admin_id",
                table: "dev_updates",
                newName: "IX_dev_updates_admin_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_dev_updates",
                table: "dev_updates",
                column: "dev_update_id");

            migrationBuilder.AddForeignKey(
                name: "FK_dev_updates_AspNetUsers_admin_id",
                table: "dev_updates",
                column: "admin_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
