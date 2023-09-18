using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace COLLATEFINAL.Data.Migrations
{
    public partial class AddedEventsPartTwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
