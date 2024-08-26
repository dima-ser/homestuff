using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeStuff.Migrations
{
    /// <inheritdoc />
    public partial class Sets2_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Location",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemSetId",
                table: "Item",
                type: "INTEGER",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Item_ItemSetId",
                table: "Item",
                column: "ItemSetId");

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

            migrationBuilder.DropTable(
                name: "ItemSet");

            migrationBuilder.DropIndex(
                name: "IX_Item_ItemSetId",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "ItemSetId",
                table: "Item");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Location",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
