using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaffManagmentNET.Migrations
{
    public partial class RemoveDeviceFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Staffs_StaffID",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_StaffID",
                table: "Devices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Devices_StaffID",
                table: "Devices",
                column: "StaffID");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Staffs_StaffID",
                table: "Devices",
                column: "StaffID",
                principalTable: "Staffs",
                principalColumn: "StaffID");
        }
    }
}
