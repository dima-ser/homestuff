using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeStuff.Migrations
{
    /// <inheritdoc />
    public partial class Relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Item_LocationId",
                table: "Item",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Location_LocationId",
                table: "Item",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Location_LocationId",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_LocationId",
                table: "Item");
        }
    }
}
