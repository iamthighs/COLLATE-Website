using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace COLLATEFINAL.Data.Migrations
{
    public partial class AddedEventsPartThree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Events_EventsModelId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EventsModelId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EventsModelId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "AttendanceEvents",
                columns: table => new
                {
                    AttendeesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EventsAttendanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceEvents", x => new { x.AttendeesId, x.EventsAttendanceId });
                    table.ForeignKey(
                        name: "FK_AttendanceEvents_AspNetUsers_AttendeesId",
                        column: x => x.AttendeesId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendanceEvents_Events_EventsAttendanceId",
                        column: x => x.EventsAttendanceId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceEvents_EventsAttendanceId",
                table: "AttendanceEvents",
                column: "EventsAttendanceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceEvents");

            migrationBuilder.AddColumn<int>(
                name: "EventsModelId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EventsModelId",
                table: "AspNetUsers",
                column: "EventsModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Events_EventsModelId",
                table: "AspNetUsers",
                column: "EventsModelId",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}
