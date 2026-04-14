using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eLab.DAL.Migrations
{
    /// <inheritdoc />
    public partial class editasdasd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Prices_TestCatalogId",
                table: "Prices");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_TestCatalogId",
                table: "Prices",
                column: "TestCatalogId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Prices_TestCatalogId",
                table: "Prices");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_TestCatalogId",
                table: "Prices",
                column: "TestCatalogId");
        }
    }
}
