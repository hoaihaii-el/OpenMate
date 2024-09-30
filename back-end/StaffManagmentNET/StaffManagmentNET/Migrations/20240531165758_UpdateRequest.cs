using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaffManagmentNET.Migrations
{
    public partial class UpdateRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestAcceptDetails_Staffs_ManagerID",
                table: "RequestAcceptDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestCreateDetails_Requests_RequestID",
                table: "RequestCreateDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestCreateDetails_Staffs_StaffID",
                table: "RequestCreateDetails");

            migrationBuilder.DropIndex(
                name: "IX_RequestCreateDetails_RequestID",
                table: "RequestCreateDetails");

            migrationBuilder.DropIndex(
                name: "IX_RequestCreateDetails_StaffID",
                table: "RequestCreateDetails");

            migrationBuilder.DropIndex(
                name: "IX_RequestAcceptDetails_ManagerID",
                table: "RequestAcceptDetails");

            migrationBuilder.RenameColumn(
                name: "AcceptTime",
                table: "RequestAcceptDetails",
                newName: "ActionTime");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "RequestAcceptDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "RequestAcceptDetails");

            migrationBuilder.RenameColumn(
                name: "ActionTime",
                table: "RequestAcceptDetails",
                newName: "AcceptTime");

            migrationBuilder.CreateIndex(
                name: "IX_RequestCreateDetails_RequestID",
                table: "RequestCreateDetails",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestCreateDetails_StaffID",
                table: "RequestCreateDetails",
                column: "StaffID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestAcceptDetails_ManagerID",
                table: "RequestAcceptDetails",
                column: "ManagerID");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestAcceptDetails_Staffs_ManagerID",
                table: "RequestAcceptDetails",
                column: "ManagerID",
                principalTable: "Staffs",
                principalColumn: "StaffID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestCreateDetails_Requests_RequestID",
                table: "RequestCreateDetails",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "RequestID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestCreateDetails_Staffs_StaffID",
                table: "RequestCreateDetails",
                column: "StaffID",
                principalTable: "Staffs",
                principalColumn: "StaffID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
