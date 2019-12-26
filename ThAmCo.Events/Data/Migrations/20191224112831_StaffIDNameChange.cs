using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Events.Data.Migrations
{
    public partial class StaffIDNameChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "thamco.events",
                table: "Staff",
                columns: new[] { "Id", "Email", "FirstAider", "Name" },
                values: new object[] { 1, "j.smith@example.com", false, "John Smith" });

            migrationBuilder.InsertData(
                schema: "thamco.events",
                table: "Staff",
                columns: new[] { "Id", "Email", "FirstAider", "Name" },
                values: new object[] { 2, "b.johnson@example.com", false, "Bill Johnson" });

            migrationBuilder.InsertData(
                schema: "thamco.events",
                table: "Staff",
                columns: new[] { "Id", "Email", "FirstAider", "Name" },
                values: new object[] { 3, "a.willings@example.com", true, "Andrew Willings" });

            migrationBuilder.InsertData(
                schema: "thamco.events",
                table: "EventStaff",
                columns: new[] { "StaffId", "EventId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 1 },
                    { 2, 2 },
                    { 3, 1 },
                    { 3, 2 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "thamco.events",
                table: "EventStaff",
                keyColumns: new[] { "StaffId", "EventId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                schema: "thamco.events",
                table: "EventStaff",
                keyColumns: new[] { "StaffId", "EventId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                schema: "thamco.events",
                table: "EventStaff",
                keyColumns: new[] { "StaffId", "EventId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                schema: "thamco.events",
                table: "EventStaff",
                keyColumns: new[] { "StaffId", "EventId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                schema: "thamco.events",
                table: "EventStaff",
                keyColumns: new[] { "StaffId", "EventId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                schema: "thamco.events",
                table: "EventStaff",
                keyColumns: new[] { "StaffId", "EventId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                schema: "thamco.events",
                table: "Staff",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "thamco.events",
                table: "Staff",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "thamco.events",
                table: "Staff",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
