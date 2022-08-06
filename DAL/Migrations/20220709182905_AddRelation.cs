using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class AddRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemAncRelations_AncIngredients_AncIngredientId",
                table: "ItemAncRelations");

            migrationBuilder.DropIndex(
                name: "IX_ItemAncRelations_AncIngredientId",
                table: "ItemAncRelations");
        }
    }
}
