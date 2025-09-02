using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReportService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFilePathFromReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Reports");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "Reports",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReportContacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactId = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    ReportId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportContacts_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportContacts_ReportId",
                table: "ReportContacts",
                column: "ReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportContacts");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Reports");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Reports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Reports",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
