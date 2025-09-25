using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beacon.Migrations
{
    /// <inheritdoc />
    public partial class FixVoteDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "alert_post_votes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AlertPostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AlertPostAlertId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alert_post_votes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_alert_post_votes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_alert_post_votes_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_alert_post_votes_alertPost_AlertPostAlertId",
                        column: x => x.AlertPostAlertId,
                        principalTable: "alertPost",
                        principalColumn: "alert_id");
                    table.ForeignKey(
                        name: "FK_alert_post_votes_alertPost_AlertPostId",
                        column: x => x.AlertPostId,
                        principalTable: "alertPost",
                        principalColumn: "alert_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Complains",
                columns: table => new
                {
                    ComplaintId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ComplaintImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    VoteCount = table.Column<int>(type: "int", nullable: false),
                    Resolved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complains", x => x.ComplaintId);
                    table.ForeignKey(
                        name: "FK_Complains_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dev_update_votes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DevUpdateId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DevUpdateId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dev_update_votes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dev_update_votes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dev_update_votes_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_dev_update_votes_dev_updates_DevUpdateId",
                        column: x => x.DevUpdateId,
                        principalTable: "dev_updates",
                        principalColumn: "dev_update_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dev_update_votes_dev_updates_DevUpdateId1",
                        column: x => x.DevUpdateId1,
                        principalTable: "dev_updates",
                        principalColumn: "dev_update_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_alert_post_votes_AlertPostAlertId",
                table: "alert_post_votes",
                column: "AlertPostAlertId");

            migrationBuilder.CreateIndex(
                name: "IX_alert_post_votes_AlertPostId",
                table: "alert_post_votes",
                column: "AlertPostId");

            migrationBuilder.CreateIndex(
                name: "IX_alert_post_votes_UserId",
                table: "alert_post_votes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_alert_post_votes_UserId1",
                table: "alert_post_votes",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Complains_UserId",
                table: "Complains",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_dev_update_votes_DevUpdateId",
                table: "dev_update_votes",
                column: "DevUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_dev_update_votes_DevUpdateId1",
                table: "dev_update_votes",
                column: "DevUpdateId1");

            migrationBuilder.CreateIndex(
                name: "IX_dev_update_votes_UserId",
                table: "dev_update_votes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_dev_update_votes_UserId1",
                table: "dev_update_votes",
                column: "UserId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alert_post_votes");

            migrationBuilder.DropTable(
                name: "Complains");

            migrationBuilder.DropTable(
                name: "dev_update_votes");
        }
    }
}
