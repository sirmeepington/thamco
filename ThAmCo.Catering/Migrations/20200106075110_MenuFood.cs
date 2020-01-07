using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Catering.Migrations
{
    public partial class MenuFood : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MenuFood_MenuId",
                schema: "thamco.catering",
                table: "MenuFood",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuFood_Food_FoodId",
                schema: "thamco.catering",
                table: "MenuFood",
                column: "FoodId",
                principalSchema: "thamco.catering",
                principalTable: "Food",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuFood_Menus_MenuId",
                schema: "thamco.catering",
                table: "MenuFood",
                column: "MenuId",
                principalSchema: "thamco.catering",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuFood_Food_FoodId",
                schema: "thamco.catering",
                table: "MenuFood");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuFood_Menus_MenuId",
                schema: "thamco.catering",
                table: "MenuFood");

            migrationBuilder.DropIndex(
                name: "IX_MenuFood_MenuId",
                schema: "thamco.catering",
                table: "MenuFood");
        }
    }
}
