using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeStuff.Migrations
{
    /// <inheritdoc />
    public partial class AddGoneMissing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGone",
                table: "Item",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMissing",
                table: "Item",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGone",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "IsMissing",
                table: "Item");
        }
    }
}
