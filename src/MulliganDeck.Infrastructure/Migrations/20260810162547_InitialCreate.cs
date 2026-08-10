using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MulliganDeck.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardKeyword",
                columns: table => new
                {
                    CardsOracleId = table.Column<Guid>(type: "uuid", nullable: false),
                    KeywordsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardKeyword", x => new { x.CardsOracleId, x.KeywordsId });
                    table.ForeignKey(
                        name: "FK_CardKeyword_Keywords_KeywordsId",
                        column: x => x.KeywordsId,
                        principalTable: "Keywords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    OracleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    OracleText = table.Column<string>(type: "text", nullable: false),
                    ManaCost = table.Column<string>(type: "text", nullable: false),
                    Cmc = table.Column<decimal>(type: "numeric", nullable: false),
                    Colors = table.Column<string>(type: "text", nullable: false),
                    ColorIdentity = table.Column<string>(type: "text", nullable: false),
                    TypeLine = table.Column<string>(type: "text", nullable: false),
                    Power = table.Column<string>(type: "text", nullable: true),
                    Toughness = table.Column<string>(type: "text", nullable: true),
                    DefaultPrintingId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.OracleId);
                });

            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Format = table.Column<string>(type: "text", nullable: false),
                    CommanderId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Decks_Cards_CommanderId",
                        column: x => x.CommanderId,
                        principalTable: "Cards",
                        principalColumn: "OracleId");
                });

            migrationBuilder.CreateTable(
                name: "Legalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Format = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CardId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Legalities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Legalities_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "OracleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Printings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Set = table.Column<string>(type: "text", nullable: false),
                    SetName = table.Column<string>(type: "text", nullable: false),
                    CollectorNumber = table.Column<string>(type: "text", nullable: false),
                    Rarity = table.Column<string>(type: "text", nullable: false),
                    ImageUri = table.Column<string>(type: "text", nullable: true),
                    Artist = table.Column<string>(type: "text", nullable: true),
                    FlavorText = table.Column<string>(type: "text", nullable: true),
                    PriceEur = table.Column<decimal>(type: "numeric", nullable: true),
                    CardmarketId = table.Column<int>(type: "integer", nullable: true),
                    Foil = table.Column<bool>(type: "boolean", nullable: false),
                    NonFoil = table.Column<bool>(type: "boolean", nullable: false),
                    CardId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Printings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Printings_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "OracleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Foil = table.Column<bool>(type: "boolean", nullable: false),
                    PrintingId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectionItems_Printings_PrintingId",
                        column: x => x.PrintingId,
                        principalTable: "Printings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeckCard",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    DeckId = table.Column<int>(type: "integer", nullable: false),
                    CardId = table.Column<Guid>(type: "uuid", nullable: false),
                    PrintingId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeckCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeckCard_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "OracleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeckCard_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeckCard_Printings_PrintingId",
                        column: x => x.PrintingId,
                        principalTable: "Printings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardKeyword_KeywordsId",
                table: "CardKeyword",
                column: "KeywordsId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_DefaultPrintingId",
                table: "Cards",
                column: "DefaultPrintingId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionItems_PrintingId",
                table: "CollectionItems",
                column: "PrintingId");

            migrationBuilder.CreateIndex(
                name: "IX_DeckCard_CardId",
                table: "DeckCard",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_DeckCard_DeckId",
                table: "DeckCard",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_DeckCard_PrintingId",
                table: "DeckCard",
                column: "PrintingId");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_CommanderId",
                table: "Decks",
                column: "CommanderId");

            migrationBuilder.CreateIndex(
                name: "IX_Legalities_CardId",
                table: "Legalities",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Printings_CardId",
                table: "Printings",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_CardKeyword_Cards_CardsOracleId",
                table: "CardKeyword",
                column: "CardsOracleId",
                principalTable: "Cards",
                principalColumn: "OracleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Printings_DefaultPrintingId",
                table: "Cards",
                column: "DefaultPrintingId",
                principalTable: "Printings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Printings_Cards_CardId",
                table: "Printings");

            migrationBuilder.DropTable(
                name: "CardKeyword");

            migrationBuilder.DropTable(
                name: "CollectionItems");

            migrationBuilder.DropTable(
                name: "DeckCard");

            migrationBuilder.DropTable(
                name: "Legalities");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropTable(
                name: "Decks");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Printings");
        }
    }
}
