using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeStuff.Migrations
{
    /// <inheritdoc />
    public partial class Sets2_DeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_ItemSet_ItemSetId",
                table: "Item");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_ItemSet_ItemSetId",
                table: "Item",
                column: "ItemSetId",
                principalTable: "ItemSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_ItemSet_ItemSetId",
                table: "Item");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_ItemSet_ItemSetId",
                table: "Item",
                column: "ItemSetId",
                principalTable: "ItemSet",
                principalColumn: "Id");
        }
    }
}
