using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eLab.DAL.Migrations
{
    /// <inheritdoc />
    public partial class editresultmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Results_BookingItemId",
                table: "Results");

            migrationBuilder.CreateIndex(
                name: "IX_Results_BookingItemId",
                table: "Results",
                column: "BookingItemId",
                unique: true,
                filter: "[BookingItemId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Results_BookingItemId",
                table: "Results");

            migrationBuilder.CreateIndex(
                name: "IX_Results_BookingItemId",
                table: "Results",
                column: "BookingItemId");
        }
    }
}
