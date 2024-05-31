using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaffManagmentNET.Migrations
{
    public partial class AddBHXHBHYT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BHTN",
                table: "Staffs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BHXH",
                table: "Staffs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BHYT",
                table: "Staffs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HDLD",
                table: "Staffs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MaSoThue",
                table: "Staffs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SoHDLD",
                table: "Staffs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BHTN",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "BHXH",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "BHYT",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "HDLD",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "MaSoThue",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "SoHDLD",
                table: "Staffs");
        }
    }
}
