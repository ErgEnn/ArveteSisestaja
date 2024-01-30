using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceNo = table.Column<string>(type: "TEXT", nullable: false),
                    InvoiceSender = table.Column<string>(type: "TEXT", nullable: false),
                    InvoiceDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    XML = table.Column<string>(type: "TEXT", nullable: true),
                    PdfSrc = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => new { x.InvoiceNo, x.InvoiceSender, x.InvoiceDateTime });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoices");
        }
    }
}
