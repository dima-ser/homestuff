using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeStuff.Migrations
{
    /// <inheritdoc />
    public partial class SetsRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Set_SetId",
                table: "Item");

            migrationBuilder.DropTable(
                name: "Set");

            migrationBuilder.CreateTable(
                name: "ItemSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSet", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Item_ItemSet_SetId",
                table: "Item",
                column: "SetId",
                principalTable: "ItemSet",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_ItemSet_SetId",
                table: "Item");

            migrationBuilder.DropTable(
                name: "ItemSet");

            migrationBuilder.CreateTable(
                name: "Set",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Set", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Set_SetId",
                table: "Item",
                column: "SetId",
                principalTable: "Set",
                principalColumn: "Id");
        }
    }
}
