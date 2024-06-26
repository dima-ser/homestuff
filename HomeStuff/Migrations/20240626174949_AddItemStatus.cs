using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeStuff.Migrations
{
    /// <inheritdoc />
    public partial class AddItemStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGone",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "IsMissing",
                table: "Item",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Item",
                newName: "IsMissing");

            migrationBuilder.AddColumn<bool>(
                name: "IsGone",
                table: "Item",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
