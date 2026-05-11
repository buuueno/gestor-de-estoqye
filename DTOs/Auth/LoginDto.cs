using System.ComponentModel.DataAnnotations;

namespace ControleEstoqueApi.DTOs.Auth;

public class LoginDto {
    [Required(ErrorMessage = "Username é obrigatório.")]
    [MinLength(3, ErrorMessage = "Username deve ter no mínimo 3 caracteres.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória.")]
    [MinLength(6, ErrorMessage = "Senha deve ter no mínimo 6 caracteres.")]
    public string Password { get; set; } = string.Empty;
}
