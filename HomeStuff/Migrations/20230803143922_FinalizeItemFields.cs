using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeStuff.Migrations
{
    /// <inheritdoc />
    public partial class FinalizeItemFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "Item",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelNumber",
                table: "Item",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "PurchaseDate",
                table: "Item",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "Item",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Item",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                table: "Item",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "ModelNumber",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "SKU",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "Item");
        }
    }
}
