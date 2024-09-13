using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceOwnsServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Services_InvoiceId",
                table: "Services",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Invoices_InvoiceId",
                table: "Services",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Invoices_InvoiceId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_InvoiceId",
                table: "Services");
        }
    }
}
