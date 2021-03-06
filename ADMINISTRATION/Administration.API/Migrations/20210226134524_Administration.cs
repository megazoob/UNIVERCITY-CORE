﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Administration.API.Migrations
{
    public partial class Administration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfStaffUnits = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Abolished = table.Column<bool>(type: "bit", nullable: false),
                    AbolishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubordinateToId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Departments_SubordinateToId",
                        column: x => x.SubordinateToId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict,
                        onUpdate: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_SubordinateToId",
                table: "Departments",
                column: "SubordinateToId");
        }

        //protected override void Down(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.DropTable(
        //        name: "Departments");
        //}
    }
}
