using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Beacon.Migrations
{
    /// <inheritdoc />
    public partial class AddCanonicalLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "canonical_location",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name_en = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_canonical_location", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "canonical_location",
                columns: new[] { "id", "name_en" },
                values: new object[,]
                {
                    { 1, "Mirpur" },
                    { 2, "Gulshan" },
                    { 3, "Uttara" },
                    { 4, "Dhanmondi" },
                    { 5, "Jatrabari" },
                    { 6, "Badda" },
                    { 7, "Banani" },
                    { 8, "Mohammadpur" },
                    { 9, "Motijheel" },
                    { 10, "Tejgaon" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_canonical_location_name_en",
                table: "canonical_location",
                column: "name_en",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "canonical_location");
        }
    }
}
