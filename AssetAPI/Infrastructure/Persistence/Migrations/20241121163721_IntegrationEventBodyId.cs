using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IntegrationEventBodyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BodyId",
                table: "Outbox",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BodyId",
                table: "Outbox");
        }
    }
}
