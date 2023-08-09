using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeStuff.Migrations
{
    /// <inheritdoc />
    public partial class Maintenance4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Maintenance_ItemId",
                table: "Maintenance",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Maintenance_Item_ItemId",
                table: "Maintenance",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maintenance_Item_ItemId",
                table: "Maintenance");

            migrationBuilder.DropIndex(
                name: "IX_Maintenance_ItemId",
                table: "Maintenance");
        }
    }
}
