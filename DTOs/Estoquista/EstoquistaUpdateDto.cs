using System.ComponentModel.DataAnnotations;

namespace ControleEstoqueApi.DTOs.Estoquista;

public class EstoquistaUpdateDto {

    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório.")]
    [MinLength(3,   ErrorMessage = "Nome deve ter entre 3 e 100 caracteres.")]
    [MaxLength(100, ErrorMessage = "Nome deve ter entre 3 e 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Email em formato inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Idade é obrigatória.")]
    [Range(18, int.MaxValue, ErrorMessage = "Idade mínima é 18 anos.")]
    public int Idade { get; set; }
}
