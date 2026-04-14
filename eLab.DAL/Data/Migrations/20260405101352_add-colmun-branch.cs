using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eLab.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addcolmunbranch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "PatientProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PatientProfiles_BranchId",
                table: "PatientProfiles",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientProfiles_Branches_BranchId",
                table: "PatientProfiles",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientProfiles_Branches_BranchId",
                table: "PatientProfiles");

            migrationBuilder.DropIndex(
                name: "IX_PatientProfiles_BranchId",
                table: "PatientProfiles");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "PatientProfiles");
        }
    }
}
