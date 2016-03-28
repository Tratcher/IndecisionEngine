using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace IndecisionEngine.Migrations.StoryDb
{
    public partial class State : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InitialState",
                table: "StorySeed",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "InitialState", table: "StorySeed");
        }
    }
}
