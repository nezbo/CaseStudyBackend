using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RichIntEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Outbox",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "Outbox",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "Outbox");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Outbox");
        }
    }
}
