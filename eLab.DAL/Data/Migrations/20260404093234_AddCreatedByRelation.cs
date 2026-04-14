using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eLab.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffProfiles_User_CreatedById",
                table: "StaffProfiles");

            migrationBuilder.DropColumn(
                name: "VisibleToPatient",
                table: "PatientRecords");

            migrationBuilder.DropColumn(
                name: "VisibleToStaff",
                table: "PatientRecords");

            migrationBuilder.DropColumn(
                name: "DateOfBrith",
                table: "PatientProfiles");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "PatientProfiles");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "StaffProfiles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StaffProfiles_UserId1",
                table: "StaffProfiles",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffProfiles_User_CreatedById",
                table: "StaffProfiles",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StaffProfiles_User_UserId1",
                table: "StaffProfiles",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffProfiles_User_CreatedById",
                table: "StaffProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_StaffProfiles_User_UserId1",
                table: "StaffProfiles");

            migrationBuilder.DropIndex(
                name: "IX_StaffProfiles_UserId1",
                table: "StaffProfiles");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "StaffProfiles");

            migrationBuilder.AddColumn<bool>(
                name: "VisibleToPatient",
                table: "PatientRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "VisibleToStaff",
                table: "PatientRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBrith",
                table: "PatientProfiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "PatientProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffProfiles_User_CreatedById",
                table: "StaffProfiles",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
