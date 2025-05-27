using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceAPI.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "InvoiceDbContext");

            migrationBuilder.CreateTable(
                name: "InvoiceDbContext_Invoices",
                schema: "InvoiceDbContext",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IssuingDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Year = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Month = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Total = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDbContext_Invoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceDbContext_Outbox",
                schema: "InvoiceDbContext",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Version = table.Column<string>(type: "TEXT", nullable: false),
                    Body = table.Column<string>(type: "TEXT", nullable: false),
                    BodyId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Source = table.Column<string>(type: "TEXT", nullable: true),
                    TraceId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDbContext_Outbox", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceDbContext_Services",
                schema: "InvoiceDbContext",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AssetId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    ValidFrom = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    ValidTo = table.Column<DateOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDbContext_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceDbContext_Services_InvoiceDbContext_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "InvoiceDbContext",
                        principalTable: "InvoiceDbContext_Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDbContext_Services_InvoiceId",
                schema: "InvoiceDbContext",
                table: "InvoiceDbContext_Services",
                column: "InvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceDbContext_Outbox",
                schema: "InvoiceDbContext");

            migrationBuilder.DropTable(
                name: "InvoiceDbContext_Services",
                schema: "InvoiceDbContext");

            migrationBuilder.DropTable(
                name: "InvoiceDbContext_Invoices",
                schema: "InvoiceDbContext");
        }
    }
}
