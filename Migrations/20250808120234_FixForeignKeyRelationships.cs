using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeSheetAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKeyRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectLevels_Projects_ProjectId1",
                table: "ProjectLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_ProjectLevels_LevelId1",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_LevelId1",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectLevels_ProjectId1",
                table: "ProjectLevels");

            migrationBuilder.DropColumn(
                name: "LevelId1",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "ProjectId1",
                table: "ProjectLevels");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LevelId1",
                table: "ProjectTasks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId1",
                table: "ProjectLevels",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_LevelId1",
                table: "ProjectTasks",
                column: "LevelId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectLevels_ProjectId1",
                table: "ProjectLevels",
                column: "ProjectId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectLevels_Projects_ProjectId1",
                table: "ProjectLevels",
                column: "ProjectId1",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_ProjectLevels_LevelId1",
                table: "ProjectTasks",
                column: "LevelId1",
                principalTable: "ProjectLevels",
                principalColumn: "Id");
        }
    }
}
