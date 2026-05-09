using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleEstoqueApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTipoProdutoRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoProdutoId",
                table: "Produtos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_TipoProdutoId",
                table: "Produtos",
                column: "TipoProdutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_TiposProduto_TipoProdutoId",
                table: "Produtos",
                column: "TipoProdutoId",
                principalTable: "TiposProduto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_TiposProduto_TipoProdutoId",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_TipoProdutoId",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "TipoProdutoId",
                table: "Produtos");
        }
    }
}
