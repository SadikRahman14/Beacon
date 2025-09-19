using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beacon.Migrations
{
    /// <inheritdoc />
    public partial class AddFaqTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "faq",
                columns: table => new
                {
                    faq_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    question = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    answer = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faq", x => x.faq_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "faq");
        }
    }
}
