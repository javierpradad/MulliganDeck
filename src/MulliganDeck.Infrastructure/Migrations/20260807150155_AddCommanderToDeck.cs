using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MulliganDeck.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCommanderToDeck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CommanderId",
                table: "Decks",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Decks_CommanderId",
                table: "Decks",
                column: "CommanderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_Cards_CommanderId",
                table: "Decks",
                column: "CommanderId",
                principalTable: "Cards",
                principalColumn: "OracleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Cards_CommanderId",
                table: "Decks");

            migrationBuilder.DropIndex(
                name: "IX_Decks_CommanderId",
                table: "Decks");

            migrationBuilder.DropColumn(
                name: "CommanderId",
                table: "Decks");
        }
    }
}
