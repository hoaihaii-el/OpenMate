using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaffManagmentNET.Migrations
{
    public partial class CreateDBAndInitTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatRooms",
                columns: table => new
                {
                    RoomID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RoomName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRooms", x => x.RoomID);
                });

            migrationBuilder.CreateTable(
                name: "Divisions",
                columns: table => new
                {
                    DivisionID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DivisionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManagerID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divisions", x => x.DivisionID);
                });

            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    StaffID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    StaffName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartWork = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartFullTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Male = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PersonalEmail = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ManagerID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DivisionID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.StaffID);
                    table.ForeignKey(
                        name: "FK_Staffs_Divisions_DivisionID",
                        column: x => x.DivisionID,
                        principalTable: "Divisions",
                        principalColumn: "DivisionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Staffs_Staffs_ManagerID",
                        column: x => x.ManagerID,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onUpdate: ReferentialAction.NoAction,
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ChatRoomsDetail",
                columns: table => new
                {
                    RoomID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StaffID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRoomsDetail", x => new { x.StaffID, x.RoomID });
                    table.ForeignKey(
                        name: "FK_ChatRoomsDetail_ChatRooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "ChatRooms",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatRoomsDetail_Staffs_StaffID",
                        column: x => x.StaffID,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    DeviceID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StaffID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.DeviceID);
                    table.ForeignKey(
                        name: "FK_Devices_Staffs_StaffID",
                        column: x => x.StaffID,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FamilyInfoes",
                columns: table => new
                {
                    StaffID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Relationship = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YearBirth = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyInfoes", x => new { x.StaffID, x.Order });
                    table.ForeignKey(
                        name: "FK_FamilyInfoes_Staffs_StaffID",
                        column: x => x.StaffID,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RoomID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResourceURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SendTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageID);
                    table.ForeignKey(
                        name: "FK_Messages_ChatRooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "ChatRooms",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Staffs_SenderID",
                        column: x => x.SenderID,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotiID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NotiName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatorID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotiID);
                    table.ForeignKey(
                        name: "FK_Notifications_Staffs_CreatorID",
                        column: x => x.CreatorID,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskDetails",
                columns: table => new
                {
                    Date = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StaffID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    TaskName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskDetails", x => new { x.Date, x.StaffID });
                    table.ForeignKey(
                        name: "FK_TaskDetails_Staffs_StaffID",
                        column: x => x.StaffID,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeSheets",
                columns: table => new
                {
                    Date = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StaffID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CheckIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LunchBreakHour = table.Column<int>(type: "int", nullable: false),
                    WorkingType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSheets", x => new { x.Date, x.StaffID });
                    table.ForeignKey(
                        name: "FK_TimeSheets_Staffs_StaffID",
                        column: x => x.StaffID,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomsDetail_RoomID",
                table: "ChatRoomsDetail",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_StaffID",
                table: "Devices",
                column: "StaffID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RoomID",
                table: "Messages",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderID",
                table: "Messages",
                column: "SenderID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatorID",
                table: "Notifications",
                column: "CreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_DivisionID",
                table: "Staffs",
                column: "DivisionID");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_ManagerID",
                table: "Staffs",
                column: "ManagerID");

            migrationBuilder.CreateIndex(
                name: "IX_TaskDetails_StaffID",
                table: "TaskDetails",
                column: "StaffID");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSheets_StaffID",
                table: "TimeSheets",
                column: "StaffID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatRoomsDetail");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "FamilyInfoes");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "TaskDetails");

            migrationBuilder.DropTable(
                name: "TimeSheets");

            migrationBuilder.DropTable(
                name: "ChatRooms");

            migrationBuilder.DropTable(
                name: "Staffs");

            migrationBuilder.DropTable(
                name: "Divisions");
        }
    }
}
