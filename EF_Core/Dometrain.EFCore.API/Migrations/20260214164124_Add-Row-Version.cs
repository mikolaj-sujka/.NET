using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dometrain.EFCore.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ConcurrencyToken",
                table: "Genres",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_AgeRating",
                table: "Pictures",
                column: "AgeRating",
                descending: new bool[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pictures_AgeRating",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "ConcurrencyToken",
                table: "Genres");
        }
    }
}
