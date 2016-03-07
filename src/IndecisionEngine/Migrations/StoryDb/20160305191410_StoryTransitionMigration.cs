using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace IndecisionEngine.Migrations.StoryDb
{
    public partial class StoryTransitionMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoryTransition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChoiceId = table.Column<int>(nullable: true),
                    Effects = table.Column<string>(nullable: true),
                    NextEntryId = table.Column<long>(nullable: true),
                    Preconditions = table.Column<string>(nullable: true),
                    PriorEntryId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryTransition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoryTransition_StoryChoice_ChoiceId",
                        column: x => x.ChoiceId,
                        principalTable: "StoryChoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoryTransition_StoryEntry_NextEntryId",
                        column: x => x.NextEntryId,
                        principalTable: "StoryEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoryTransition_StoryEntry_PriorEntryId",
                        column: x => x.PriorEntryId,
                        principalTable: "StoryEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("StoryTransition");
        }
    }
}
