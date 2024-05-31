using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaffManagmentNET.Migrations
{
    public partial class UpdateTaskDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TaskDetails",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<double>(
                name: "Estimate",
                table: "TaskDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Evaluate",
                table: "TaskDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "TaskDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "TaskDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estimate",
                table: "TaskDetails");

            migrationBuilder.DropColumn(
                name: "Evaluate",
                table: "TaskDetails");

            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "TaskDetails");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "TaskDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TaskDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);
        }
    }
}
