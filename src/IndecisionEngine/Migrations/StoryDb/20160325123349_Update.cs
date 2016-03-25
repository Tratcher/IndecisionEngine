using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace IndecisionEngine.Migrations.StoryDb
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "StoryEntryId", table: "StorySeed");
            migrationBuilder.AddColumn<int>(
                name: "FirstEntryId",
                table: "StorySeed",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "FirstEntryId", table: "StorySeed");
            migrationBuilder.AddColumn<int>(
                name: "StoryEntryId",
                table: "StorySeed",
                nullable: true);
        }
    }
}
