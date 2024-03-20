using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeStuff.Migrations
{
    /// <inheritdoc />
    public partial class SetsRename2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_ItemSet_SetId",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "SetId",
                table: "Item",
                newName: "ItemSetId");

            migrationBuilder.RenameIndex(
                name: "IX_Item_SetId",
                table: "Item",
                newName: "IX_Item_ItemSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_ItemSet_ItemSetId",
                table: "Item",
                column: "ItemSetId",
                principalTable: "ItemSet",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_ItemSet_ItemSetId",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "ItemSetId",
                table: "Item",
                newName: "SetId");

            migrationBuilder.RenameIndex(
                name: "IX_Item_ItemSetId",
                table: "Item",
                newName: "IX_Item_SetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_ItemSet_SetId",
                table: "Item",
                column: "SetId",
                principalTable: "ItemSet",
                principalColumn: "Id");
        }
    }
}
