using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beacon.Migrations
{
    /// <inheritdoc />
    public partial class AddAlertComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "alert_comments",
                columns: table => new
                {
                    comment_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    alert_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    content = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alert_comments", x => x.comment_id);
                    table.ForeignKey(
                        name: "FK_alert_comments_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_alert_comments_alertPost_alert_id",
                        column: x => x.alert_id,
                        principalTable: "alertPost",
                        principalColumn: "alert_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_alert_comments_alert_id",
                table: "alert_comments",
                column: "alert_id");

            migrationBuilder.CreateIndex(
                name: "IX_alert_comments_user_id",
                table: "alert_comments",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alert_comments");
        }
    }
}
