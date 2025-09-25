using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beacon.Migrations
{
    /// <inheritdoc />
    public partial class FixVoteModelsClean : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_alert_post_votes_AspNetUsers_UserId1",
                table: "alert_post_votes");

            migrationBuilder.DropForeignKey(
                name: "FK_alert_post_votes_alertPost_AlertPostAlertId",
                table: "alert_post_votes");

            migrationBuilder.DropForeignKey(
                name: "FK_dev_update_votes_AspNetUsers_UserId",
                table: "dev_update_votes");

            migrationBuilder.DropForeignKey(
                name: "FK_dev_update_votes_AspNetUsers_UserId1",
                table: "dev_update_votes");

            migrationBuilder.DropForeignKey(
                name: "FK_dev_update_votes_dev_updates_DevUpdateId",
                table: "dev_update_votes");

            migrationBuilder.DropForeignKey(
                name: "FK_dev_update_votes_dev_updates_DevUpdateId1",
                table: "dev_update_votes");

            migrationBuilder.DropIndex(
                name: "IX_alert_post_votes_AlertPostAlertId",
                table: "alert_post_votes");

            migrationBuilder.DropIndex(
                name: "IX_alert_post_votes_AlertPostId",
                table: "alert_post_votes");

            migrationBuilder.DropIndex(
                name: "IX_alert_post_votes_UserId1",
                table: "alert_post_votes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dev_update_votes",
                table: "dev_update_votes");

            migrationBuilder.DropIndex(
                name: "IX_dev_update_votes_DevUpdateId",
                table: "dev_update_votes");

            migrationBuilder.DropIndex(
                name: "IX_dev_update_votes_DevUpdateId1",
                table: "dev_update_votes");

            migrationBuilder.DropIndex(
                name: "IX_dev_update_votes_UserId1",
                table: "dev_update_votes");

            migrationBuilder.DropColumn(
                name: "AlertPostAlertId",
                table: "alert_post_votes");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "alert_post_votes");

            migrationBuilder.DropColumn(
                name: "DevUpdateId1",
                table: "dev_update_votes");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "dev_update_votes");

            migrationBuilder.RenameTable(
                name: "dev_update_votes",
                newName: "DevUpdateVotes");

            migrationBuilder.RenameIndex(
                name: "IX_dev_update_votes_UserId",
                table: "DevUpdateVotes",
                newName: "IX_DevUpdateVotes_UserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "alert_post_votes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "alert_post_votes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "DevUpdateVotes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "DevUpdateVotes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DevUpdateVotes",
                table: "DevUpdateVotes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_alertPost_created_at",
                table: "alertPost",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_alert_post_votes_AlertPostId_UserId",
                table: "alert_post_votes",
                columns: new[] { "AlertPostId", "UserId" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_AlertPostVote_Value",
                table: "alert_post_votes",
                sql: "[Value] IN (-1, 1)");

            migrationBuilder.CreateIndex(
                name: "IX_DevUpdateVotes_DevUpdateId_UserId",
                table: "DevUpdateVotes",
                columns: new[] { "DevUpdateId", "UserId" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_DevUpdateVote_Value",
                table: "DevUpdateVotes",
                sql: "[Value] IN (-1, 1)");

            migrationBuilder.AddForeignKey(
                name: "FK_DevUpdateVotes_AspNetUsers_UserId",
                table: "DevUpdateVotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DevUpdateVotes_dev_updates_DevUpdateId",
                table: "DevUpdateVotes",
                column: "DevUpdateId",
                principalTable: "dev_updates",
                principalColumn: "dev_update_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DevUpdateVotes_AspNetUsers_UserId",
                table: "DevUpdateVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_DevUpdateVotes_dev_updates_DevUpdateId",
                table: "DevUpdateVotes");

            migrationBuilder.DropIndex(
                name: "IX_alertPost_created_at",
                table: "alertPost");

            migrationBuilder.DropIndex(
                name: "IX_alert_post_votes_AlertPostId_UserId",
                table: "alert_post_votes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_AlertPostVote_Value",
                table: "alert_post_votes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DevUpdateVotes",
                table: "DevUpdateVotes");

            migrationBuilder.DropIndex(
                name: "IX_DevUpdateVotes_DevUpdateId_UserId",
                table: "DevUpdateVotes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_DevUpdateVote_Value",
                table: "DevUpdateVotes");

            migrationBuilder.RenameTable(
                name: "DevUpdateVotes",
                newName: "dev_update_votes");

            migrationBuilder.RenameIndex(
                name: "IX_DevUpdateVotes_UserId",
                table: "dev_update_votes",
                newName: "IX_dev_update_votes_UserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "alert_post_votes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "alert_post_votes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "AlertPostAlertId",
                table: "alert_post_votes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "alert_post_votes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "dev_update_votes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "dev_update_votes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "DevUpdateId1",
                table: "dev_update_votes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "dev_update_votes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_dev_update_votes",
                table: "dev_update_votes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_alert_post_votes_AlertPostAlertId",
                table: "alert_post_votes",
                column: "AlertPostAlertId");

            migrationBuilder.CreateIndex(
                name: "IX_alert_post_votes_AlertPostId",
                table: "alert_post_votes",
                column: "AlertPostId");

            migrationBuilder.CreateIndex(
                name: "IX_alert_post_votes_UserId1",
                table: "alert_post_votes",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_dev_update_votes_DevUpdateId",
                table: "dev_update_votes",
                column: "DevUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_dev_update_votes_DevUpdateId1",
                table: "dev_update_votes",
                column: "DevUpdateId1");

            migrationBuilder.CreateIndex(
                name: "IX_dev_update_votes_UserId1",
                table: "dev_update_votes",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_alert_post_votes_AspNetUsers_UserId1",
                table: "alert_post_votes",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_alert_post_votes_alertPost_AlertPostAlertId",
                table: "alert_post_votes",
                column: "AlertPostAlertId",
                principalTable: "alertPost",
                principalColumn: "alert_id");

            migrationBuilder.AddForeignKey(
                name: "FK_dev_update_votes_AspNetUsers_UserId",
                table: "dev_update_votes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_dev_update_votes_AspNetUsers_UserId1",
                table: "dev_update_votes",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_dev_update_votes_dev_updates_DevUpdateId",
                table: "dev_update_votes",
                column: "DevUpdateId",
                principalTable: "dev_updates",
                principalColumn: "dev_update_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dev_update_votes_dev_updates_DevUpdateId1",
                table: "dev_update_votes",
                column: "DevUpdateId1",
                principalTable: "dev_updates",
                principalColumn: "dev_update_id");
        }
    }
}
