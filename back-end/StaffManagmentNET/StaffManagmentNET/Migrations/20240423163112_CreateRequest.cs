using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaffManagmentNET.Migrations
{
    public partial class CreateRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestAcceptDetails",
                columns: table => new
                {
                    DetailID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManagerID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AcceptTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestAcceptDetails", x => new { x.DetailID, x.ManagerID });
                    table.ForeignKey(
                        name: "FK_RequestAcceptDetails_Staffs_ManagerID",
                        column: x => x.ManagerID,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    RequestID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequestType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcceptLevel = table.Column<int>(type: "int", nullable: false),
                    ExternalAcceptID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.RequestID);
                });

            migrationBuilder.CreateTable(
                name: "RequestCreateDetails",
                columns: table => new
                {
                    DetailID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequestID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StaffID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Evidence = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentLevel = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestCreateDetails", x => x.DetailID);
                    table.ForeignKey(
                        name: "FK_RequestCreateDetails_Requests_RequestID",
                        column: x => x.RequestID,
                        principalTable: "Requests",
                        principalColumn: "RequestID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestCreateDetails_Staffs_StaffID",
                        column: x => x.StaffID,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestAcceptDetails_ManagerID",
                table: "RequestAcceptDetails",
                column: "ManagerID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestCreateDetails_RequestID",
                table: "RequestCreateDetails",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestCreateDetails_StaffID",
                table: "RequestCreateDetails",
                column: "StaffID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestAcceptDetails");

            migrationBuilder.DropTable(
                name: "RequestCreateDetails");

            migrationBuilder.DropTable(
                name: "Requests");
        }
    }
}
