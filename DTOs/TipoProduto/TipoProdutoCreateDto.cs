using System.ComponentModel.DataAnnotations;

namespace ControleEstoqueApi.DTOs.TipoProduto;

public class TipoProdutoCreateDto {

    [Required(ErrorMessage = "Nome do tipo de produto é obrigatório.")]
    [MinLength(3,  ErrorMessage = "Nome deve ter entre 3 e 80 caracteres.")]
    [MaxLength(80, ErrorMessage = "Nome deve ter entre 3 e 80 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(200, ErrorMessage = "Descrição deve ter no máximo 200 caracteres.")]
    public string? Descricao { get; set; }
}
