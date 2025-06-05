using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceAPI.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ServiceSimplified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidFrom",
                schema: "InvoiceDbContext",
                table: "InvoiceDbContext_Services");

            migrationBuilder.DropColumn(
                name: "ValidTo",
                schema: "InvoiceDbContext",
                table: "InvoiceDbContext_Services");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                schema: "InvoiceDbContext",
                table: "InvoiceDbContext_Services",
                type: "INTEGER",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                schema: "InvoiceDbContext",
                table: "InvoiceDbContext_Services");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ValidFrom",
                schema: "InvoiceDbContext",
                table: "InvoiceDbContext_Services",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ValidTo",
                schema: "InvoiceDbContext",
                table: "InvoiceDbContext_Services",
                type: "TEXT",
                nullable: true);
        }
    }
}
