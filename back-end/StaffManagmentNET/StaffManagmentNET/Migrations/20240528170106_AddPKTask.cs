using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaffManagmentNET.Migrations
{
    public partial class AddPKTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskDetails",
                table: "TaskDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskDetails",
                table: "TaskDetails",
                columns: new[] { "Date", "StaffID", "Order" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskDetails",
                table: "TaskDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskDetails",
                table: "TaskDetails",
                columns: new[] { "Date", "StaffID" });
        }
    }
}
