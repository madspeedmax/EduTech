using Microsoft.EntityFrameworkCore.Migrations;

namespace StudyReg.Web.Data.Migrations
{
    public partial class CardsInDeck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Card_Deck_DeckId",
                table: "Card");

            migrationBuilder.DropIndex(
                name: "IX_Card_DeckId",
                table: "Card");

            migrationBuilder.DropColumn(
                name: "DeckId",
                table: "Card");

            migrationBuilder.CreateTable(
                name: "DeckCard",
                columns: table => new
                {
                    CardId = table.Column<int>(nullable: false),
                    DeckId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeckCard", x => new { x.CardId, x.DeckId });
                    table.ForeignKey(
                        name: "FK_DeckCard_Card_CardId",
                        column: x => x.CardId,
                        principalTable: "Card",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeckCard_Deck_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Deck",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeckCard_DeckId",
                table: "DeckCard",
                column: "DeckId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeckCard");

            migrationBuilder.AddColumn<int>(
                name: "DeckId",
                table: "Card",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Card_DeckId",
                table: "Card",
                column: "DeckId");

            migrationBuilder.AddForeignKey(
                name: "FK_Card_Deck_DeckId",
                table: "Card",
                column: "DeckId",
                principalTable: "Deck",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
