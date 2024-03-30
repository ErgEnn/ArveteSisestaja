using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp.Migrations
{
    /// <inheritdoc />
    public partial class pdffile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PdfSrc",
                table: "Invoices");

            migrationBuilder.AddColumn<byte[]>(
                name: "Pdf",
                table: "Invoices",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pdf",
                table: "Invoices");

            migrationBuilder.AddColumn<string>(
                name: "PdfSrc",
                table: "Invoices",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
