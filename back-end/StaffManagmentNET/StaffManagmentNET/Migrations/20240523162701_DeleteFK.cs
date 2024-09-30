using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaffManagmentNET.Migrations
{
    public partial class DeleteFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Staffs_CreatorID",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_CreatorID",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "CreatorID",
                table: "Notifications");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorID",
                table: "Notifications",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatorID",
                table: "Notifications",
                column: "CreatorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Staffs_CreatorID",
                table: "Notifications",
                column: "CreatorID",
                principalTable: "Staffs",
                principalColumn: "StaffID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
