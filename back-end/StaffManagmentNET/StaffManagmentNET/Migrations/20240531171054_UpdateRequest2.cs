using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaffManagmentNET.Migrations
{
    public partial class UpdateRequest2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentLevel",
                table: "RequestCreateDetails");

            migrationBuilder.RenameColumn(
                name: "DetailID",
                table: "RequestCreateDetails",
                newName: "CreateID");

            migrationBuilder.RenameColumn(
                name: "DetailID",
                table: "RequestAcceptDetails",
                newName: "CreateID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateID",
                table: "RequestCreateDetails",
                newName: "DetailID");

            migrationBuilder.RenameColumn(
                name: "CreateID",
                table: "RequestAcceptDetails",
                newName: "DetailID");

            migrationBuilder.AddColumn<int>(
                name: "CurrentLevel",
                table: "RequestCreateDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
