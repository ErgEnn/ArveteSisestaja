using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class EditAncIngredient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemAncRelations_AncIngredients_AncIngredientId",
                table: "ItemAncRelations");

            migrationBuilder.DropIndex(
                name: "IX_ItemAncRelations_AncIngredientId",
                table: "ItemAncRelations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AncIngredients",
                table: "AncIngredients");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AncIngredients");

            migrationBuilder.AddColumn<string>(
                name: "AncIngredientName",
                table: "ItemAncRelations",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AncIngredients",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AncIngredients",
                table: "AncIngredients",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ItemAncRelations_AncIngredientName",
                table: "ItemAncRelations",
                column: "AncIngredientName");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemAncRelations_AncIngredients_AncIngredientName",
                table: "ItemAncRelations",
                column: "AncIngredientName",
                principalTable: "AncIngredients",
                principalColumn: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemAncRelations_AncIngredients_AncIngredientName",
                table: "ItemAncRelations");

            migrationBuilder.DropIndex(
                name: "IX_ItemAncRelations_AncIngredientName",
                table: "ItemAncRelations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AncIngredients",
                table: "AncIngredients");

            migrationBuilder.DropColumn(
                name: "AncIngredientName",
                table: "ItemAncRelations");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AncIngredients",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AncIngredients",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AncIngredients",
                table: "AncIngredients",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ItemAncRelations_AncIngredientId",
                table: "ItemAncRelations",
                column: "AncIngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemAncRelations_AncIngredients_AncIngredientId",
                table: "ItemAncRelations",
                column: "AncIngredientId",
                principalTable: "AncIngredients",
                principalColumn: "Id");
        }
    }
}
