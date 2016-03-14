using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace IndecisionEngine.Migrations.StoryDb
{
    public partial class setup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoryChoice",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Body = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryChoice", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "StoryEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Body = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryEntry", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "StorySeed",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StoryEntryId = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorySeed", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "StoryTransition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChoiceId = table.Column<int>(nullable: true),
                    Effects = table.Column<string>(nullable: true),
                    NextEntryId = table.Column<int>(nullable: true),
                    Preconditions = table.Column<string>(nullable: true),
                    PriorEntryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryTransition", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("StoryChoice");
            migrationBuilder.DropTable("StoryEntry");
            migrationBuilder.DropTable("StorySeed");
            migrationBuilder.DropTable("StoryTransition");
        }
    }
}
