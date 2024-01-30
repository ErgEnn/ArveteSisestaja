using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp.Migrations
{
    /// <inheritdoc />
    public partial class _5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AncClassifierMappings",
                table: "AncClassifierMappings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AncClassifierMappings",
                table: "AncClassifierMappings",
                column: "ProductName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AncClassifierMappings",
                table: "AncClassifierMappings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AncClassifierMappings",
                table: "AncClassifierMappings",
                columns: new[] { "AncClassifierId", "ProductName" });
        }
    }
}
