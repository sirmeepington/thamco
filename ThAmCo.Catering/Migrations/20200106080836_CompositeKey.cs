﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Catering.Migrations
{
    public partial class CompositeKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Food_Menus_MenuId",
                schema: "thamco.catering",
                table: "Food");

            migrationBuilder.DropIndex(
                name: "IX_Food_MenuId",
                schema: "thamco.catering",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "MenuId",
                schema: "thamco.catering",
                table: "Food");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                schema: "thamco.catering",
                table: "Food",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Food_MenuId",
                schema: "thamco.catering",
                table: "Food",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Food_Menus_MenuId",
                schema: "thamco.catering",
                table: "Food",
                column: "MenuId",
                principalSchema: "thamco.catering",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
