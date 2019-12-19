using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Events.Data.Migrations
{
    public partial class EventStaff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FirstAider",
                schema: "thamco.events",
                table: "Staff",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "EventStaff",
                schema: "thamco.events",
                columns: table => new
                {
                    StaffId = table.Column<int>(nullable: false),
                    EventId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStaff", x => new { x.StaffId, x.EventId });
                    table.UniqueConstraint("AK_EventStaff_EventId_StaffId", x => new { x.EventId, x.StaffId });
                    table.ForeignKey(
                        name: "FK_EventStaff_Events_EventId",
                        column: x => x.EventId,
                        principalSchema: "thamco.events",
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventStaff_Staff_StaffId",
                        column: x => x.StaffId,
                        principalSchema: "thamco.events",
                        principalTable: "Staff",
                        principalColumn: "StaffId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventStaff",
                schema: "thamco.events");

            migrationBuilder.DropColumn(
                name: "FirstAider",
                schema: "thamco.events",
                table: "Staff");
        }
    }
}
