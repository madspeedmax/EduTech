using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StudyReg.Web.Data.Migrations
{
    public partial class selfassessment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goal_AspNetUsers_UserId",
                table: "Goal");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Goal",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Goal",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Goal",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SelfAssessmentAdjustment",
                table: "Goal",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SelfAssessmentDate",
                table: "Goal",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SelfAssessmentExpectation",
                table: "Goal",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Goal_AspNetUsers_UserId",
                table: "Goal",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goal_AspNetUsers_UserId",
                table: "Goal");

            migrationBuilder.DropColumn(
                name: "SelfAssessmentAdjustment",
                table: "Goal");

            migrationBuilder.DropColumn(
                name: "SelfAssessmentDate",
                table: "Goal");

            migrationBuilder.DropColumn(
                name: "SelfAssessmentExpectation",
                table: "Goal");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Goal",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Goal",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Goal",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Goal_AspNetUsers_UserId",
                table: "Goal",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
