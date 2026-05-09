namespace ControleEstoqueApi.DTOs.Estoquista;

public class EstoquistaDto {
    public int    Id    { get; set; }
    public string Nome  { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int    Idade { get; set; }
}
