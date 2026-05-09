namespace ControleEstoqueApi.DTOs.TipoProduto;

public class TipoProdutoDto {
    public int     Id        { get; set; }
    public string  Nome      { get; set; } = string.Empty;
    public string? Descricao { get; set; }
}
