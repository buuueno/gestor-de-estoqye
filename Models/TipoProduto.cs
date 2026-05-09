namespace ControleEstoqueApi.Models;

public class TipoProduto {
    public int     Id        { get; set; }
    public string  Nome      { get; set; } = string.Empty;
    public string? Descricao { get; set; }

    // Relacionamento: um tipo tem muitos produtos
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
