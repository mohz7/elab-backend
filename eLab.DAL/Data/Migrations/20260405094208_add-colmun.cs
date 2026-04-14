using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eLab.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addcolmun : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Allergies",
                table: "PatientProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BloodType",
                table: "PatientProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChronicDiseases",
                table: "PatientProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactName",
                table: "PatientProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactPhone",
                table: "PatientProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceCompany",
                table: "PatientProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceNumber",
                table: "PatientProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "PatientProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Allergies",
                table: "PatientProfiles");

            migrationBuilder.DropColumn(
                name: "BloodType",
                table: "PatientProfiles");

            migrationBuilder.DropColumn(
                name: "ChronicDiseases",
                table: "PatientProfiles");

            migrationBuilder.DropColumn(
                name: "EmergencyContactName",
                table: "PatientProfiles");

            migrationBuilder.DropColumn(
                name: "EmergencyContactPhone",
                table: "PatientProfiles");

            migrationBuilder.DropColumn(
                name: "InsuranceCompany",
                table: "PatientProfiles");

            migrationBuilder.DropColumn(
                name: "InsuranceNumber",
                table: "PatientProfiles");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "PatientProfiles");
        }
    }
}
