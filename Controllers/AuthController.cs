// Controle Auth
using Microsoft.AspNetCore.Mvc;
using ControleEstoqueApi.DTOs.Auth;
using ControleEstoqueApi.Services;
using ControleEstoqueApi.Models;

namespace ControleEstoqueApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase {
    private readonly AuthService _authService;

    public AuthController(AuthService authService) {
        _authService = authService;
    }

  
    [HttpPost("login")]
    public IActionResult Login(LoginDto dto) {
        var response = _authService.Login(dto);

        if (string.IsNullOrEmpty(response.Token)) {
            return Unauthorized(ApiResponse<object>.BadRequest(response.Message));
        }

        return Ok(ApiResponse<AuthResponseDto>.Ok(response, response.Message));
    }
}
