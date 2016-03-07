using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace IndecisionEngine.Migrations.StoryDb
{
    public partial class StoryEntries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_StoryTransition_StoryChoice_ChoiceId", table: "StoryTransition");
            migrationBuilder.DropForeignKey(name: "FK_StoryTransition_StoryEntry_NextEntryId", table: "StoryTransition");
            migrationBuilder.DropForeignKey(name: "FK_StoryTransition_StoryEntry_PriorEntryId", table: "StoryTransition");
            migrationBuilder.AlterColumn<int>(
                name: "PriorEntryId",
                table: "StoryTransition",
                nullable: true);
            migrationBuilder.AlterColumn<int>(
                name: "NextEntryId",
                table: "StoryTransition",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "PriorEntryId",
                table: "StoryTransition",
                nullable: true);
            migrationBuilder.AlterColumn<long>(
                name: "NextEntryId",
                table: "StoryTransition",
                nullable: true);
            migrationBuilder.AddForeignKey(
                name: "FK_StoryTransition_StoryChoice_ChoiceId",
                table: "StoryTransition",
                column: "ChoiceId",
                principalTable: "StoryChoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_StoryTransition_StoryEntry_NextEntryId",
                table: "StoryTransition",
                column: "NextEntryId",
                principalTable: "StoryEntry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_StoryTransition_StoryEntry_PriorEntryId",
                table: "StoryTransition",
                column: "PriorEntryId",
                principalTable: "StoryEntry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
