using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eLab.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddResultIdToStaffChats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResultId",
                table: "StaffChats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StaffChats_ResultId",
                table: "StaffChats",
                column: "ResultId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StaffChats_Results_ResultId",
                table: "StaffChats",
                column: "ResultId",
                principalTable: "Results",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffChats_Results_ResultId",
                table: "StaffChats");

            migrationBuilder.DropIndex(
                name: "IX_StaffChats_ResultId",
                table: "StaffChats");

            migrationBuilder.DropColumn(
                name: "ResultId",
                table: "StaffChats");
        }
    }
}
