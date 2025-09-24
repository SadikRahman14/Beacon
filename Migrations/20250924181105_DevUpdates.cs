using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beacon.Migrations
{
    /// <inheritdoc />
    public partial class DevUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dev_updates",
                columns: table => new
                {
                    dev_update_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    admin_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    image_url = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    content = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dev_updates", x => x.dev_update_id);
                    table.ForeignKey(
                        name: "FK_dev_updates_AspNetUsers_admin_id",
                        column: x => x.admin_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dev_updates_admin_id",
                table: "dev_updates",
                column: "admin_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dev_updates");
        }
    }
}
