using Microsoft.EntityFrameworkCore.Migrations;

namespace StudyReg.Web.Data.Migrations
{
    public partial class studylogrequiredfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyLog_Card_CardId",
                table: "StudyLog");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyLog_AspNetUsers_UserId",
                table: "StudyLog");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "StudyLog",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CardId",
                table: "StudyLog",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyLog_Card_CardId",
                table: "StudyLog",
                column: "CardId",
                principalTable: "Card",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyLog_AspNetUsers_UserId",
                table: "StudyLog",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyLog_Card_CardId",
                table: "StudyLog");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyLog_AspNetUsers_UserId",
                table: "StudyLog");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "StudyLog",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "CardId",
                table: "StudyLog",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_StudyLog_Card_CardId",
                table: "StudyLog",
                column: "CardId",
                principalTable: "Card",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyLog_AspNetUsers_UserId",
                table: "StudyLog",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
