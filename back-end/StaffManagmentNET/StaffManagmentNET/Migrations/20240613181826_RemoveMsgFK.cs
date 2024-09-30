using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaffManagmentNET.Migrations
{
    public partial class RemoveMsgFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ChatRooms_RoomID",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Staffs_SenderID",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_RoomID",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SenderID",
                table: "Messages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Messages_RoomID",
                table: "Messages",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderID",
                table: "Messages",
                column: "SenderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ChatRooms_RoomID",
                table: "Messages",
                column: "RoomID",
                principalTable: "ChatRooms",
                principalColumn: "RoomID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Staffs_SenderID",
                table: "Messages",
                column: "SenderID",
                principalTable: "Staffs",
                principalColumn: "StaffID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
