using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeStuff.Migrations
{
    /// <inheritdoc />
    public partial class Sets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SetId",
                table: "Item",
                type: "INTEGER",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Item_SetId",
                table: "Item",
                column: "SetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Set_SetId",
                table: "Item",
                column: "SetId",
                principalTable: "Set",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Set_SetId",
                table: "Item");

            migrationBuilder.DropTable(
                name: "Set");

            migrationBuilder.DropIndex(
                name: "IX_Item_SetId",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "SetId",
                table: "Item");
        }
    }
}
