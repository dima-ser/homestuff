using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeStuff.Migrations
{
    /// <inheritdoc />
    public partial class Maintenance6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Maintenance");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Maintenance",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Maintenance",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Maintenance",
                type: "TEXT",
                nullable: true);
        }
    }
}
