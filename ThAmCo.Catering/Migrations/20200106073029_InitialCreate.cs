using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Catering.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "thamco.catering");

            migrationBuilder.CreateTable(
                name: "Food",
                schema: "thamco.catering",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Cost = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Food", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                schema: "thamco.catering",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuFood",
                schema: "thamco.catering",
                columns: table => new
                {
                    FoodId = table.Column<int>(nullable: false),
                    MenuId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuFood", x => new { x.FoodId, x.MenuId });
                    table.ForeignKey(
                        name: "FK_MenuFood_Menus_MenuId",
                        column: x => x.MenuId,
                        principalSchema: "thamco.catering",
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "thamco.catering",
                table: "Food",
                columns: new[] { "Id", "Cost", "Name" },
                values: new object[,]
                {
                    { 1, 1.23f, "Prawn Cocktail" },
                    { 2, 4.23f, "Carbonara Basile" },
                    { 3, 5.32f, "Rabbit Haunch" },
                    { 4, 6.22f, "Steak & Kidney Pie" }
                });

            migrationBuilder.InsertData(
                schema: "thamco.catering",
                table: "Menus",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Spring Formality" });

            migrationBuilder.InsertData(
                schema: "thamco.catering",
                table: "MenuFood",
                columns: new[] { "FoodId", "MenuId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuFood_MenuId",
                schema: "thamco.catering",
                table: "MenuFood",
                column: "MenuId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Food",
                schema: "thamco.catering");

            migrationBuilder.DropTable(
                name: "MenuFood",
                schema: "thamco.catering");

            migrationBuilder.DropTable(
                name: "Menus",
                schema: "thamco.catering");
        }
    }
}
