using System.ComponentModel.DataAnnotations;

namespace ControleEstoqueApi.DTOs.Produto;

public class ProdutoUpdateDto {

    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório.")]
    [MinLength(3,   ErrorMessage = "Nome deve ter entre 3 e 120 caracteres.")]
    [MaxLength(120, ErrorMessage = "Nome deve ter entre 3 e 120 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero.")]
    public decimal Preco { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Estoque não pode ser negativo.")]
    public int Estoque { get; set; }

    [Required(ErrorMessage = "TipoProdutoId é obrigatório.")]
    public int TipoProdutoId { get; set; }
}
