namespace ControleEstoqueApi.Models;

public class Produto {
    public int     Id            { get; set; }
    public string  Nome          { get; set; } = string.Empty;
    public decimal Preco         { get; set; }   // Preço unitário
    public int     Estoque       { get; set; }   // Quantidade em estoque

    // Chave estrangeira para TipoProduto
    public int     TipoProdutoId { get; set; }
    public TipoProduto? TipoProduto { get; set; }
}
