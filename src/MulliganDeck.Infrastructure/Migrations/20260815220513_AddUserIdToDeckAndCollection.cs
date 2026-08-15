using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MulliganDeck.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToDeckAndCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Decks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CollectionItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Decks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CollectionItems");
        }
    }
}
