using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beacon.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationSeedStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "location_seed_stats",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    location_id = table.Column<int>(type: "int", nullable: false),
                    alerts = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    complaints = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    dev_updates = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_location_seed_stats", x => x.id);
                    table.ForeignKey(
                        name: "FK_location_seed_stats_canonical_location_location_id",
                        column: x => x.location_id,
                        principalTable: "canonical_location",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_location_seed_stats_location_id",
                table: "location_seed_stats",
                column: "location_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "location_seed_stats");
        }
    }
}
