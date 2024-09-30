using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaffManagmentNET.Migrations
{
    public partial class RemoveManageFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Staffs_ManagerID",
                table: "Staffs");

            migrationBuilder.DropIndex(
                name: "IX_Staffs_ManagerID",
                table: "Staffs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Staffs_ManagerID",
                table: "Staffs",
                column: "ManagerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Staffs_ManagerID",
                table: "Staffs",
                column: "ManagerID",
                principalTable: "Staffs",
                principalColumn: "StaffID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
