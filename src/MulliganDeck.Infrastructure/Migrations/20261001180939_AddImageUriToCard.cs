using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MulliganDeck.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUriToCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUri",
                table: "Cards",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUri",
                table: "Cards");
        }
    }
}
