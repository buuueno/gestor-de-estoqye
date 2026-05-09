namespace ControleEstoqueApi.DTOs.Produto;

public class ProdutoDto {
    public int     Id             { get; set; }
    public string  Nome           { get; set; } = string.Empty;
    public decimal Preco          { get; set; }
    public int     Estoque        { get; set; }
    public int     TipoProdutoId  { get; set; }
    public string  TipoProdutoNome { get; set; } = string.Empty;
}
