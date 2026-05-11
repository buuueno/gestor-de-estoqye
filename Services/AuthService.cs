using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ControleEstoqueApi.DTOs.Auth;

namespace ControleEstoqueApi.Services;

public class AuthService {
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration) {
        _configuration = configuration;
    }

    /// <summary>
    /// Realiza login e gera um JWT token.
    /// Para desenvolvimento, aceita username="admin" e password="123456"
    /// </summary>
    public AuthResponseDto Login(LoginDto dto) {
        
        if (dto.Username != "admin" || dto.Password != "123456") {
            return new AuthResponseDto {
                Message = "Username ou senha inválidos.",
                Token = string.Empty,
                ExpiresIn = 0
            };
        }

        // Gerar JWT
        var token = GenerateJwtToken(dto.Username);
        var expiresIn = _configuration.GetValue<int>("Jwt:ExpiresInSeconds", 3600);

        return new AuthResponseDto {
            Token = token,
            ExpiresIn = expiresIn,
            Message = "Login realizado com sucesso!"
        };
    }

    /// <summary>
    /// Gera um JWT token válido
    /// </summary>
    private string GenerateJwtToken(string username) {
        var key = _configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(key)) {
            throw new InvalidOperationException("Jwt:Key não está configurada em appsettings.json");
        }

        var keyBytes = Encoding.UTF8.GetBytes(key);
        var securityKey = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.NameIdentifier, username),
            new Claim("role", "user")
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(_configuration.GetValue<int>("Jwt:ExpiresInSeconds", 3600)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
