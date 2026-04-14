using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eLab.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addcolumnInBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Bookings");
        }
    }
}
