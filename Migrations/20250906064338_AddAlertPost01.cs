using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beacon.Migrations
{
    /// <inheritdoc />
    public partial class AddAlertPost01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "alertPost",
                columns: table => new
                {
                    alert_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    admin_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    alert_image_url = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alertPost", x => x.alert_id);
                    table.ForeignKey(
                        name: "FK_alertPost_AspNetUsers_admin_id",
                        column: x => x.admin_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_alertPost_admin_id",
                table: "alertPost",
                column: "admin_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alertPost");
        }
    }
}
